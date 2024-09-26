using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using TxtTags.Common;

namespace TxtTags
{
    public class Repo: NotifyPropertyBase, IComparable<Repo>
    {
        private int _id;
        private bool _hide = false;
        private string _path;
        private string _name;
        //private BookTags _tags;

        public int Id { get => _id; set { _id = value; OnPropertyChanged("Id"); } }
        public string Path { get => _path; set { _path = value; OnPropertyChanged("Path"); } }
        public string Name { get => _name; set { _name = value; OnPropertyChanged("Name"); } }
        public bool Hide { get => _hide; set { _hide = value; OnPropertyChanged("Hide"); } }
        //public BookTags Tags { get => _tags; set { _tags = value; OnPropertyChanged("Tags"); } }

        public int CompareTo(Repo other)
        {
            return Path.CompareTo(other.Name);
        }
        public static void Copy(ref Repo to, Repo from)
        {
            to.Id = from.Id;
            to.Path = from.Path;
            to.Name = from.Name;
            to.Hide = from.Hide;
        }
        public Repo Clone()
        {
            var to = new Repo()
            {
                Id = Id,
                Path = Path,
                Name = Name,
                Hide = Hide
            };

            return to;
        }
    }
}
