using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07
{
    class NoteBookRecord
    {

        //DateTime createDate;
        string createDate = "";
        string title = "";
        string comment = "";
        string author = "";
        bool finished = false;
        int index = 0;


        public NoteBookRecord(string title="", string comment="", int index = -1)
        {
            //this.createDate = DateTime.Now;
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
