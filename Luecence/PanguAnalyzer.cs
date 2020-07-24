using Lucene.Net.Analysis; 
using PanGu;
using System.Collections.Generic;
using System.IO;

namespace LuecenceTest
{
    public class PanGuAnalyzer : Analyzer
    {
        public PanGuAnalyzer()
        {
        }
        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            TokenStream result = new PanGuTokenizer(reader);
            result = new LowerCaseFilter(result);
            return result;
        }
    }
    public class PanGuTokenizer : Tokenizer
    {
        static object _LockObj = new object();
        static bool _Inited = false;
        WordInfo[] _WordList;
        int _Position = -1;
        string _InputText;
        static private void InitPanGuSegment() { if (!_Inited) { PanGu.Segment.Init(); _Inited = true; } }
        public PanGuTokenizer()
        {
            lock (_LockObj)
            {
                InitPanGuSegment();
            }
        }
        public PanGuTokenizer(TextReader input)
            : base(input)
        {
            lock (_LockObj)
            {
                InitPanGuSegment();
            }
            _InputText = base.input.ReadToEnd();
            if (string.IsNullOrEmpty(_InputText))
            {
                _WordList = new WordInfo[0];
            }
            else
            {
                PanGu.Segment segment = new Segment();
                ICollection<WordInfo> wordInfos = segment.DoSegment(_InputText);
                _WordList = new WordInfo[wordInfos.Count];
                wordInfos.CopyTo(_WordList, 0);
            }
        }
        public override Token Next()
        {
            int length = 0;    //词汇的长度.72  
            int start = 0;     //开始偏移量.73  74 
            while (true)
            {
                _Position++;
                if (_Position < _WordList.Length)
                {
                    if (_WordList[_Position] != null)
                    {
                        length = _WordList[_Position].Word.Length;
                        start = _WordList[_Position].Position;
                        return new Token(_WordList[_Position].Word, start, start + length);
                    }
                }
                else
                {
                    break;
                }
            }
            _InputText = null;
            return null;
        }
    }
}