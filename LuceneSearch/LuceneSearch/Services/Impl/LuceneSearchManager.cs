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

        }

        private event EventHandler<EventDataArgs> _indexAddedEvent;
        public event EventHandler<EventDataArgs> IndexAddedEvent
        {
            add
            {
                _indexAddedEvent += LuceneSearchManager__indexAddedEvent;
            }
            remove
            {
                _indexAddedEvent -= LuceneSearchManager__indexAddedEvent;
            }
        }

        private void LuceneSearchManager__indexAddedEvent(object sender, EventDataArgs e)
        {
            
        }

        SearchContext _searchContext = null;
        
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
                            IFilesScanner fileScanner = new FileScanner();

                            //yield return call
                            foreach (var doc in fileScanner.GetFileListWithFullPath(context.ScanPath))
                            {
                                if(doc ==null)
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
                                indexWriter.AddDocument(document);
                                Trace.WriteLine(string.Format("Added {0} to Index", doc.FileName));
                                //currStatus = string.Format("Added {0} to Index", doc.FilePath);

                                //this.IndexAddedEvent?.Invoke(this, new EventDataArgs { Data = string.Format("Added {0} to Index", doc.FilePath) });

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
            catch (Exception ex)
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
                        var topDocs = indexSearcher.Search(query,10000);

                        foreach (var item in topDocs.ScoreDocs)
                        {
                            var luDoc = indexSearcher.Doc(item.Doc);

                            documentDataList.Add(new DocumentData { FileName = luDoc.Get("name"), Extention=luDoc.Get("ext"), FilePath = luDoc.Get("path") });
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
    }
}
