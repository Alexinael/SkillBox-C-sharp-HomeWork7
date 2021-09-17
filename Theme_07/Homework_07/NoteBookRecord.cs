using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07
{
    [Serializable]
    public class NoteBookRecord
    {

        public string createDate = "";
        public string title = "";
        public string comment = "";
        public string author = "";
        public bool finished = false;
        public int index = 0;

        public NoteBookRecord()
        {
            this.title = "";
            this.index = 1;
            this.comment = "empty";
            this.author = "local";
            this.createDate = "";
            this.finished = false;
        }
        //[Serializable]

       // [OnSerializing]
        public string ser()
        {
            string a = "";
            return "";
        }
        public NoteBookRecord(string title="", string comment="", int index = -1)
        {
            this.createDate = "";
            this.title = title;
            this.comment = comment;
            this.author = "local"; // machine name
            if (index == -1)
            {
                index = 0;
            }
            this.finished = false;
        }
        public string getRecord()
        {
            return $"{this.title} от {this.createDate} [{this.author}]\n\t{this.comment}";
        }
    }
}
