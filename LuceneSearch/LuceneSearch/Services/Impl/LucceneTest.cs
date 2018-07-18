using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneSearch.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LuceneSearch.Services.Impl
{
    class LucceneTest
    {
        SearchContext _searchContext = null;

        public LucceneTest()
        {
            _searchContext = new SearchContext { IndexPath = @"d:\temp\eqp001" };
            //if (!System.IO.Directory.Exists(_searchContext.IndexPath))
            //{
            //    System.IO.Directory.CreateDirectory(_searchContext.IndexPath);
            //}
        }
        public bool BuildIndex()
        {
            try
            {
                if (System.IO.Directory.Exists(_searchContext.IndexPath))
                {
                    System.IO.Directory.Delete(_searchContext.IndexPath, true);
                }

                var indexDirectory = FSDirectory.Open(_searchContext.IndexPath);
                var analyser = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                var indexWriter = new IndexWriter(indexDirectory, analyser, IndexWriter.MaxFieldLength.UNLIMITED);
                var indexSearcher = new IndexSearcher(indexDirectory, true);

                int index = 0;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                for (int i = 0; i < 50000; i++)
                {
                    Document document = new Document();
                    string res = string.Format("{0}{1}", "par", i);
                    Field nameField = new Field("paraname", res, Field.Store.YES, Field.Index.NOT_ANALYZED);
                    Field pathField = new Field("value", "10-07-2018 40, 11-07-2018 45, 12-07-2018 78", Field.Store.YES, Field.Index.NOT_ANALYZED);
                    document.Add(nameField);
                    document.Add(pathField);
                    indexWriter.AddDocument(document);
                    //Trace.WriteLine(string.Format("Added {0} to Index", doc.FilePath));
                    //System.IO.Path.GetDirectoryName(doc.FilePath), doc.FileName));
                }
                sw.Stop();
                var result = string.Format("Index built time {0} {1}", sw.Elapsed.Seconds, sw.Elapsed.Milliseconds);

                indexWriter.Optimize();
                analyser.Close();
                indexWriter.Dispose();
                indexDirectory.Dispose();
                MessageBox.Show(result);
            }
            catch (Exception ex)
            {
                return false;

            }



            return true;
        }

        public string Search(string random)
        {
            try
            {
                var indexDirectory = FSDirectory.Open(_searchContext.IndexPath);
                var indexSearcher = new IndexSearcher(indexDirectory);

                Stopwatch sw = new Stopwatch();
                sw.Start();

                var analyser = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "paraname", analyser);
                var query = queryParser.Parse(random);
                var topDocs = indexSearcher.Search(query, 1);
                sw.Stop();

                string result = string.Empty;
                foreach (var item in topDocs.ScoreDocs)
                {
                    var luDoc = indexSearcher.Doc(item.Doc);
                    result = "data " + random.ToString() + " " + luDoc.Get("value") + string.Format(" time {0} {1}", sw.Elapsed.Seconds, sw.Elapsed.Milliseconds);
                    //documentDataList.Add(new DocumentData { FileName = luDoc.Get("name"), FilePath = luDoc.Get("path") });
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
