using System.Data.SqlClient;

namespace BackEnd.Models

{
    public class Autos
    {
        private int _id;
        private string _marca;
        private string _modelo;
        private string _color;
        private int _ano;
        private int _km;
        private int _precio;
        private string _descripcion;
        private int _idEmpresa;
        private List<string> _fotos;


        //Properties

        public int Id { get { return _id; } set { _id = value; } }
        public string Marca { get { return _marca; } set { _marca = value; } }
        public string Modelo { get { return _modelo; } set { _modelo = value; } }
        public string Color { get { return _color; } set { _color = value; } }
        public int Ano { get { return _ano; } set { _ano = value; } }
        public int Km { get { return _km; } set { _km = value; } }
        public int Precio { get { return _precio; } set { _precio = value; } }
        public string Descripcion { get { return _descripcion; } set { _descripcion = value; } }
        public int IdEmpresa { get { return _idEmpresa; } set { _idEmpresa = value; } }
        public List<string> Fotos { get { return _fotos; } set { _fotos = value; } }





        //Constructor por defecto

        public Autos()
        {
            _id = 0;
            _marca = string.Empty;
            _modelo = string.Empty;
            _color = string.Empty;
            _ano = 0;
            _km = 0;
            _precio = 0;
            _descripcion = string.Empty;
            _idEmpresa = 0;
            _fotos = new List<string>();
        }

        //Constructor con toda la info
        public Autos(int id, string marca, string modelo, string color, int ano, int km, int precio, string descripcion, int idEmpresa, List<string> fotos)
        {
            _id = id;
            _marca = marca;
            _modelo = modelo;
            _color = color;
            _ano = ano;
            _km = km;
            _precio = precio;
            _descripcion = descripcion;
            _idEmpresa = idEmpresa;
            _fotos = fotos;

        }
    }
}


