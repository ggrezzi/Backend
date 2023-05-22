using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using BackEnd.Models;

namespace BackEnd.Repository
{
    public class ADO_Autos
    {
        public static Autos TraerAuto(int id)
        //Metodo que recibe un ID y retorna el Auto correspondiente a ese ID
        {
            var fotos = new List<string>();
            var p = new Autos();
            string connectionString = "Server=W0447;Database=buscandoAuto; Trusted_connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var comando = new SqlCommand("Select * from Fotos where idAuto =" + id, connection);
                using (SqlDataReader dr = comando.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            fotos.Add(dr.GetString(1));
                        }
                    }
                }
                connection.Close();
                connection.Open();

                comando = new SqlCommand("Select * from Autos where id =" + id , connection);
                using (SqlDataReader dr = comando.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            p = new Autos(Convert.ToInt32(dr.GetValue(0)), dr.GetString(1), dr.GetString(2), dr.GetString(3), Convert.ToInt32(dr.GetValue(4)), Convert.ToInt32(dr.GetValue(5)), Convert.ToInt32(dr.GetValue(6)), dr.GetString(7), Convert.ToInt32(dr.GetValue(8)), fotos);
                        }
                    }
                }
                connection.Close();
                return p;
            }
        }

        public static List<Autos> TraerAutosByEmpresa(int idEmpresa)
        //Metodo que recibe un idEmpresa y retorna una lista de Autos asignados a esa empresa
        {
            List<Autos> p = new List<Autos> { };
            List<string> fotos = new List<string> { };
            string connectionString = "Server=W0447;Database=buscandoAuto; Trusted_connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var comando = new SqlCommand("Select * from Autos where idEmpresa ='" + idEmpresa + "'", connection);
                using (SqlDataReader dr = comando.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            p.Add(new Autos(Convert.ToInt32(dr.GetValue(0)), dr.GetString(1), dr.GetString(2), dr.GetString(3), Convert.ToInt32(dr.GetValue(4)), Convert.ToInt32(dr.GetValue(5)), Convert.ToInt32(dr.GetValue(6)), dr.GetString(7), Convert.ToInt32(dr.GetValue(8)), fotos));
                        }
                    }
                }
                connection.Close();
                foreach(Autos auto in p)
                {
                    connection.Open();
                    fotos = new List<string> { };
                    comando = new SqlCommand("Select * from Fotos where idAuto =" + auto.Id , connection);
                    using (SqlDataReader dr = comando.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                fotos.Add(dr.GetString(1));
                            }
                        }
                    }
                    auto.Fotos = fotos;
                    connection.Close();
                }
            }
            return p;
        }

        public static bool CrearAuto(Autos auto)
        //Metodo para crear un Auto desde 0
        {
            //Primero reviso que los datos ingresados sean validos.
            string error = ValidarDatos(auto);
            if (string.IsNullOrEmpty(error))
            {
                string connectionString = "Server=W0447;Database=buscandoAuto; Trusted_connection=True;";
                var query = "INSERT into Autos values (@Marca, @Modelo, @Color, @Ano, @Km, @Precio, @Descripcion, @IdEmpresa)";
                ModificarCrearAuto(auto, query);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var comando = new SqlCommand("SELECT IDENT_CURRENT ('Autos')", connection);
                    using (SqlDataReader dr = comando.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                auto.Id = Convert.ToInt32(dr.GetValue(0));
                            }
                        }
                    }
                    connection.Close();

                    foreach (string foto in auto.Fotos)
                    {

                        connection.Open();
                        comando = new SqlCommand("INSERT into Fotos (Archivo,IdAuto) values ('" + foto + "'," + auto.Id + ")", connection);
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                            }
                        }
                        connection.Close();
                    }
                }

                
            }
            else
            {
                return false;
            }
            return true;
        }

        public static string ValidarDatos(Autos p)
        //Metodo que valida si la info de un Auto es correcta.

        {
            string error = string.Empty;
            if (p.Descripcion == "" || p.Descripcion == String.Empty)
            {
                error = "Descripcion vacio";
            }
            if (p.Marca == "" || p.Marca == String.Empty)
            {
                error = "Marca vacio";
            }
            if (p.Modelo == "" || p.Modelo == String.Empty)
            {
                error = "Modelo vacio";
            }
            if (p.Color == "" || p.Color == String.Empty)
            {
                error = "Color vacio";
            }
            if (p.Ano <= 0)
            {
                error = "Ano no puede ser 0";
            }
            if (p.Precio <= 0)
            {
                error = "El precio no puede ser 0";
            }
            if (p.IdEmpresa <= 0)
            {
                error = "IdEmpresa no puede ser 0";
            }
            else
            {
                string connectionString = "Server=W0447;Database=buscandoAuto;Trusted_connection=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var comando = new SqlCommand("Select * from Empresas where id =" + p.IdEmpresa, connection);
                    using (SqlDataReader dr = comando.ExecuteReader())
                    {
                        if (!dr.HasRows)
                        {
                            error = "La Empresa no Existe";
                        }
                    }
                    connection.Close();
                }
            }
            return error;
        }


        private static void ModificarCrearAuto(Autos p, string query)
        //Metodo interno usado para no repetir codigo al Modificar o crear un producto

        {
            string connectionString = "Server=W0447;Database=buscandoAuto; Trusted_connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var parametroMarca= new SqlParameter();
                parametroMarca.ParameterName = "Marca";
                parametroMarca.SqlDbType = System.Data.SqlDbType.Char;
                parametroMarca.Value = p.Marca;
                var parametroModelo = new SqlParameter();
                parametroModelo.ParameterName = "Modelo";
                parametroModelo.SqlDbType = System.Data.SqlDbType.Char;
                parametroModelo.Value = p.Modelo;
                var parametroColor = new SqlParameter();
                parametroColor.ParameterName = "Color";
                parametroColor.SqlDbType = System.Data.SqlDbType.Char;
                parametroColor.Value = p.Color;
                var parametroAno = new SqlParameter();
                parametroAno.ParameterName = "Ano";
                parametroAno.SqlDbType = System.Data.SqlDbType.Int;
                parametroAno.Value = p.Ano;
                var parametroKm = new SqlParameter();
                parametroKm.ParameterName = "Km";
                parametroKm.SqlDbType = System.Data.SqlDbType.Int;
                parametroKm.Value = p.Km;
                var parametroPrecio = new SqlParameter();
                parametroPrecio.ParameterName = "Precio";
                parametroPrecio.SqlDbType = System.Data.SqlDbType.Int;
                parametroPrecio.Value = p.Precio;
                var parametroDescripcion = new SqlParameter();
                parametroDescripcion.ParameterName = "Descripcion";
                parametroDescripcion.SqlDbType = System.Data.SqlDbType.Char;
                parametroDescripcion.Value = p.Descripcion;
                var parametroIdEmpresa = new SqlParameter();
                parametroIdEmpresa.ParameterName = "IdEmpresa";
                parametroIdEmpresa.SqlDbType = System.Data.SqlDbType.Int;
                parametroIdEmpresa.Value = p.IdEmpresa;
                connection.Open();
                using (SqlCommand comandoCreate = new SqlCommand(query, connection))
                {
                    comandoCreate.Parameters.Add(parametroMarca);
                    comandoCreate.Parameters.Add(parametroModelo);
                    comandoCreate.Parameters.Add(parametroColor);
                    comandoCreate.Parameters.Add(parametroAno);
                    comandoCreate.Parameters.Add(parametroKm);
                    comandoCreate.Parameters.Add(parametroPrecio);
                    comandoCreate.Parameters.Add(parametroDescripcion);
                    comandoCreate.Parameters.Add(parametroIdEmpresa);
                    comandoCreate.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static bool ModificarAuto(Autos p)
        {
            string error = ValidarDatos(p);
            if (string.IsNullOrEmpty(error))
            {
                var query = "UPDATE Autos Set Marca=@marca, Modelo=@modelo, Color=@color, Ano=@ano, Km=@km, Precio=@precio, " +
                "Descripcion=@descripcion, IdEmpresa=@idEmpresa " +
                "WHERE id=" + p.Id;
                ModificarCrearAuto(p, query);
                return true;
            }
            else
            {
                return false;
            }


        }
        public static bool EliminarAuto(int id)
        // Metodo para Eliminar un Auto dado su ID
        {
            string connectionString = "Server=W0447;Database=buscandoAuto; Trusted_connection=True;";
            string query = "DELETE FROM Autos Where id=" + id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand comandoCreate = new SqlCommand(query, connection))
                {
                    comandoCreate.ExecuteNonQuery();
                }
                connection.Close();
            }

            query = "DELETE FROM Fotos Where IdAuto=" + id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand comandoCreate = new SqlCommand(query, connection))
                {
                    comandoCreate.ExecuteNonQuery();
                }
                connection.Close();
            }
            return true;
        }
    }
}
