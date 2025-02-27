﻿using Models;
using MySql.Data.MySqlClient;
using Repository.Interfaces;

namespace Repository.MysqlServers
{
    public class ProductsRepository : Repository, IProductsRepository
    {
        public ProductsRepository(MySqlConnection connect, MySqlTransaction transaction)
        {
            this._connect = connect;
            this._transaction = transaction;
        }
        public List<Products> GetAll()
        {
            var result = new List<Products>();
            try
            {
                const string queryDb = "SELECT * FROM products";
                using (var command = CreateMySqlCommand(queryDb))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Products
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Price = reader.GetDecimal("price"),
                            });
                        }
                    }
                }
            }
            catch (MySqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public Products GetById(int id)
        {
            var result = new Products();
            try
            {
                const string queryDB = "SELECT * FROM products WHERE id = @id";
                var command = CreateMySqlCommand(queryDB);
                command.Parameters.AddWithValue("id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new Products
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Price = reader.GetDecimal("price")
                        };
                    }
                    else
                    {
                        throw new ArgumentException("El ID no se encuentra registrado");
                    }
                }
            }
            catch (MySqlException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public void Create(Products product)
        {
            try
            {
                const string queryDB = "INSERT INTO products (name, price) VALUES (@A,@B)";
                using (MySqlCommand command = CreateMySqlCommand(queryDB))
                {
                    command.Parameters.AddWithValue("@A", product.Name);
                    command.Parameters.AddWithValue("@B", product.Price);
                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(Products product)
        {
            try
            {
                const string queryDB = "UPDATE products SET name=@A, price=@B WHERE id=@productID";
                using (MySqlCommand command = CreateMySqlCommand(queryDB))
                {
                    command.Parameters.AddWithValue("@productID", product.Id);
                    command.Parameters.AddWithValue("@A", product.Name);
                    command.Parameters.AddWithValue("@B", product.Price);
                    var res = command.ExecuteNonQuery();
                    if (res < 1) throw new ArgumentException("El ID no se encuentra registrado");
                }
            }
            catch (MySqlException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(int productID)
        {
            try
            {
                const string queryDb = "DELETE FROM products WHERE id=@productID";
                using (MySqlCommand command = CreateMySqlCommand(queryDb))
                {
                    command.Parameters.AddWithValue("@productID", productID);
                    var res = command.ExecuteNonQuery();
                    if (res < 1) throw new ArgumentException("El ID no se encuentra registrado");
                }
            }
            catch (MySqlException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
