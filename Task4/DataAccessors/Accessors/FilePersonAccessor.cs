﻿using DataAccessors.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace DataAccessors.Accessors
{
    public class FilePersonAccessor: IAccessor<Person>
    {
        private static XmlSerializer PersonArraySerializer = 
            new XmlSerializer(typeof(List<Person>), new[] { typeof(Person), typeof(Phone) });

        private ICollection<Person> data;
        private string fileName;

        public FilePersonAccessor(string fileName)
        {
            this.fileName = fileName;
            try
            {
                data = DeserializeCollection();
            }
            catch
            {
                data = new LinkedList<Person>();
            }
        }

        public ICollection<Person> GetAll()
        {
            return data;
        }
        public Person GetById(object id)
        {
            var res = from p in data where p.Id == (int)id select p;
            return res.FirstOrDefault<Person>();
        }
        public void DeleteById(object id)
        {
            var res = from p in data where p.Id == (int)id select p;
            if (res.FirstOrDefault<Person>() != null)
            {
                Person existPerson = res.First<Person>();
                data.Remove(existPerson);
            }
            SerializeCollection(data);
        }
        public void Insert(Person p)
        {
            var tmp = from ep in data where ep.Id == p.Id select ep;
            Person existPerson = tmp.FirstOrDefault<Person>();
            if (existPerson != null)
            {
                data.Remove(existPerson);
            }            
            data.Add(p);
            SerializeCollection(data);
        }     

        public void SerializeCollection(ICollection<Person> collection)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                PersonArraySerializer.Serialize(sw, collection.ToList<Person>());
            }
        }
        public ICollection<Person> DeserializeCollection()
        {
            using (StreamReader sr = new StreamReader(fileName))
            {               
                return (List<Person>)PersonArraySerializer.Deserialize(sr);
            }
        }
    }
}
