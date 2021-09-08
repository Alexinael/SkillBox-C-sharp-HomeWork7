using System;

namespace Homework_07
{
    public class Option
    {

        public string Name { get; }
        public Action Selected { get; }

        public Option(string name, Action selected)
        {
            this.Name = name;
            this.Selected = selected;
        }
    }
}