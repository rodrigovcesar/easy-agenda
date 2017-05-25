
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAgenda.Models
{
    public class Usuario
    {
        string id;
        string nome;
        string email;        
        string _telefone;
        

        [JsonProperty(PropertyName = "telefone")]
        public string Telefone
        {
            get { return _telefone; }
            set { _telefone = value; }
        }


        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        [JsonProperty(PropertyName = "nome")]
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
        [JsonProperty(PropertyName = "email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        //[JsonProperty(PropertyName = "createdAt")]
        //public DateTime CreatedAt
        //{
        //    get { return createdAt; }
        //    set { createdAt = value; }
        //}
        
    }

}

