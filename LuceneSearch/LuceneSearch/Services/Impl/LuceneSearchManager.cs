using LuceneSearch.Services.Inte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuceneSearch.DataModel;
using Lucene.Net.Store;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using System.Diagnostics;
using System.Configuration;
using System.IO;

namespace LuceneSearch.Services.Impl
{
    public class LuceneSearchManager : ISearchManager
    {
        //public delegate void IndexAddedDelegate(string doc);
        //public event IndexAddedDelegate IndexAddedEvent;
        //TODO: The Index writer and the searcher can be segregated into diff classes.
        //StandardAnalyzer _analyser = null;
        //FSDirectory _indexDirectory = null;
        //IndexWriter _indexWriter = null;

        //IndexSearcher _indexSearcher = null;


        public LuceneSearchManager()
        {
            _fileScanner.FileCreatedDeletedEventHandler += _fileScanner_FileCreatedDeletedEventHandler;
            _fileScanner.FileRenamedEventHandler += _fileScanner_FileRenamedEventHandler;
            _fileScanner.SetupFileWatcher();
        }

        private void _fileScanner_FileRenamedEventHandler(object sender, RenamedEventArgs e)
        {
            RenameDocumentInIndex(e, new SearchContext { IndexPath = ConfigurationManager.AppSettings.Get("IndexLocation") });
        }

        private void _fileScanner_FileCreatedDeletedEventHandler(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    AddDocumentToIndex(e.FullPath, new SearchContext { IndexPath = ConfigurationManager.AppSettings.Get("IndexLocation") });
                    break;
                case WatcherChangeTypes.Deleted:
                    DeleteDocumentInIndex(e, new SearchContext { IndexPath = ConfigurationManager.AppSettings.Get("IndexLocation") });
                    break;
                default:
                    break;
            }
        }

        public event EventHandler<EventDataArgs> DocumentAddedEvent;


        SearchContext _searchContext = null;
        IFilesScanner _fileScanner = new FileScanner();

        public bool BuildIndex(SearchContext context)
        {
            _searchContext = context;

            try
            {
                if (System.IO.Directory.Exists(context.IndexPath))
                {
                    System.IO.Directory.Delete(context.IndexPath, true);
                }

                using (var indexDirectory = FSDirectory.Open(context.IndexPath))
                {
                    using (var analyser = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
                    {
                        using (var indexWriter = new IndexWriter(indexDirectory, analyser, IndexWriter.MaxFieldLength.UNLIMITED))
                        {
                            //yield return call
                            foreach (var doc in _fileScanner.GetFileListWithFullPath(context.ScanPath))
                            {
                                if (doc == null)
                                {
                                    continue;
                                }

                                Document document = new Document();
                                Field nameField = new Field("name", doc.FileName, Field.Store.YES, Field.Index.ANALYZED);
                                Field extField = new Field("ext", doc.Extention, Field.Store.YES, Field.Index.NOT_ANALYZED);
                                Field pathField = new Field("path", doc.FilePath, Field.Store.YES, Field.Index.NOT_ANALYZED);

                                document.Add(nameField);
                                document.Add(pathField);
                                document.Add(extField);
                                indexWriter?.AddDocument(document);
                                //Trace.WriteLine(string.Format("Added {0} to Index", doc.FileName));
                                //currStatus = string.Format("Added {0} to Index", doc.FilePath);

                                DocumentAddedEvent?.Invoke(this, new EventDataArgs { Data = string.Format("Indexing: {0}", doc.FilePath) });

                            }
                            indexWriter.Optimize();
                        }
                        analyser.Close();
                    }
                }

                //foreach (var doc in documentDataList)
                //{
                //    Document document = new Document();
                //    Field nameField = new Field("name", doc.FileName, Field.Store.YES, Field.Index.ANALYZED);
                //    Field pathField = new Field("path", doc.FilePath, Field.Store.YES, Field.Index.NOT_ANALYZED);
                //    document.Add(nameField);
                //    document.Add(pathField);
                //    indexWriter.AddDocument(document);
                //    Trace.WriteLine(string.Format("Added {0} to Index", doc.FilePath));
                //    //System.IO.Path.GetDirectoryName(doc.FilePath), doc.FileName));
                //}
                //indexWriter.Dispose();
                //indexDirectory.Dispose();
            }
            catch (Exception)
            {
                return false;

            }
            return true;
        }

        public IList<DocumentData> Search(string searchString)
        {
            IList<DocumentData> documentDataList = new List<DocumentData>();
            try
            {
                //var indexdirectory = FSDirectory.Open(_searchContext.IndexPath);
                //_indexSearcher = new IndexSearcher(indexdirectory);

                var indexLocation = ConfigurationManager.AppSettings.Get("IndexLocation");
                using (var indexDirectory = FSDirectory.Open(indexLocation))
                {
                    using (var indexSearcher = new IndexSearcher(indexDirectory))
                    {
                        var analyser = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                        QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "name", analyser);
                        var query = queryParser.Parse(searchString);

                        int searchHitCount = 1000;//default
                        int.TryParse(ConfigurationManager.AppSettings["SearchHitCount"], out searchHitCount);

                        var topDocs = indexSearcher.Search(query, searchHitCount);

                        foreach (var item in topDocs.ScoreDocs)
                        {
                            var luDoc = indexSearcher.Doc(item.Doc);

                            documentDataList.Add(new DocumentData { FileName = luDoc.Get("name"), Extention = luDoc.Get("ext"), FilePath = luDoc.Get("path") });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return documentDataList;
        }

        private bool RenameDocumentInIndex(RenamedEventArgs renArgs, SearchContext context)
        {
            using (var indexDirectory = FSDirectory.Open(context.IndexPath))
            {
                using (var analyser = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
                {
                    using (var indexWriter = new IndexWriter(indexDirectory, analyser, false, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        Document doc = new Document();
                        Field nameField = new Field("name", renArgs.Name, Field.Store.YES, Field.Index.ANALYZED);
                        Field extField = new Field("ext", Path.GetExtension(renArgs.FullPath), Field.Store.YES, Field.Index.NOT_ANALYZED);
                        Field pathField = new Field("path", renArgs.FullPath, Field.Store.YES, Field.Index.NOT_ANALYZED);
                        doc.Add(nameField);
                        doc.Add(extField);
                        doc.Add(pathField);

                        //indexWriter?.DeleteDocuments(new Term("name", renArgs.OldName.ToLower()));
                        //indexWriter?.AddDocument(doc);
                        indexWriter?.UpdateDocument(new Term("path", renArgs.OldFullPath), doc);
                        indexWriter?.Optimize();
                        DocumentAddedEvent?.Invoke(this, new EventDataArgs { Data = string.Format("File Renamed: {0}", renArgs.FullPath) });
                    }
                    analyser.Close();
                }
            }
            return true;
        }


        private bool DeleteDocumentInIndex(FileSystemEventArgs fseArgs, SearchContext context)
        {
            using (var indexDirectory = FSDirectory.Open(context.IndexPath))
            {
                using (var analyser = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
                {
                    using (var indexWriter = new IndexWriter(indexDirectory, analyser, false, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        //try query overload of DeleteDocuments
                        indexWriter?.DeleteDocuments(new Term("name", fseArgs.Name));
                        indexWriter?.Optimize();
                        DocumentAddedEvent?.Invoke(this, new EventDataArgs { Data = string.Format("File Deleted: {0}", fseArgs.FullPath) });
                    }
                    analyser.Close();
                }
            }
            return true;
        }

        public bool AddDocumentToIndex(string fullFilePath, SearchContext context)
        {
            using (var indexDirectory = FSDirectory.Open(context.IndexPath))
            {
                using (var analyser = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
                {
                    using (var indexWriter = new IndexWriter(indexDirectory, analyser, false, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        Document document = new Document { };
                        DocumentData doc = new DocumentData
                        {
                            FileName = Path.GetFileNameWithoutExtension(fullFilePath),
                            Extention = Path.GetExtension(fullFilePath),
                            FilePath = fullFilePath
                        };

                        Field nameField = new Field("name", doc.FileName, Field.Store.YES, Field.Index.ANALYZED);
                        Field extField = new Field("ext", doc.Extention, Field.Store.YES, Field.Index.NOT_ANALYZED);
                        Field pathField = new Field("path", doc.FilePath, Field.Store.YES, Field.Index.NOT_ANALYZED);
                        document.Add(nameField);
                        document.Add(extField);
                        document.Add(pathField);

                        indexWriter?.AddDocument(document);
                        indexWriter?.Optimize();
                        DocumentAddedEvent?.Invoke(this, new EventDataArgs { Data = string.Format("File Created: {0}", doc.FilePath) });
                    }
                    analyser.Close();
                }
            }

            return true;
        }
    }
}
