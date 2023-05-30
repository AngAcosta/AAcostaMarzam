using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Medicamento
    {
        public static ML.Result Add(ML.Medicamento medicamento)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.AacostaMarzamContext context = new DL.AacostaMarzamContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"MedicamentoAdd '{medicamento.Nombre}','{medicamento.Descripcion}','{medicamento.Precio}','{medicamento.Imagen}'");

                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se Inserto el Medicamento";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }


        public static ML.Result Update(ML.Medicamento medicamento)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.AacostaMarzamContext context = new DL.AacostaMarzamContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"MedicamentoUpdate '{medicamento.IdMedicamento}','{medicamento.Nombre}','{medicamento.Descripcion}','{medicamento.Precio}','{medicamento.Imagen}'");

                    if (query >= 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se Actializo el Medicamento";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Result Delete(int IdMediacemnto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.AacostaMarzamContext context = new DL.AacostaMarzamContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"MedicamentoDelete {IdMediacemnto}");

                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "No se Elimino el Medicamento";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AacostaMarzamContext context = new DL.AacostaMarzamContext())
                {
                    var query = context.Medicamentos.FromSqlRaw($"MedicamentoGetAll").ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var obj in query)
                        {
                            ML.Medicamento medicamento = new ML.Medicamento();

                            medicamento.IdMedicamento = obj.IdMedicamento;
                            medicamento.Nombre = obj.Nombre;
                            medicamento.Descripcion = obj.Descripcion;
                            medicamento.Precio = obj.Precio;
                            medicamento.Imagen = obj.Imagen;

                            result.Objects.Add(medicamento);
                        }
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public static ML.Result GetById(int IdMediacemnto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AacostaMarzamContext context = new DL.AacostaMarzamContext())
                {
                    var query = context.Medicamentos.FromSqlRaw($"MedicamentoGetById {IdMediacemnto}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Medicamento medicamento = new ML.Medicamento();

                        medicamento.IdMedicamento = query.IdMedicamento;
                        medicamento.Nombre = query.Nombre;
                        medicamento.Descripcion = query.Descripcion;
                        medicamento.Precio = query.Precio;
                        medicamento.Imagen = query.Imagen;

                        result.Object = medicamento;
                    }
                    result.Correct = true;
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Message=ex.Message;
            }
            return result;
        }
    }
}