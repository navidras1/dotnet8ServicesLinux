using ChatV1.DataAccess.Context;
using ChatV1.DataAccess.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChatV1.DataAccess.CommonModels;
using System.Data.Common;
using Npgsql;

namespace ChatV1.DataAccess.Repository
{
    public interface IChatV1Repository<T> where T : class
    {
        T Add(T entity);
        List<T> AddRange(IEnumerable<T> entities);
        bool Exists(Expression<Func<T, bool>> filter);
        IQueryable<T> Find(Expression<Func<T, bool>> filter);
        IQueryable<T> GetAll();
        T GetById(object id);
        void Remove(T entity);
        void RemoveById(object id);
        void RemoveRange(IEnumerable<T> entities);
        void SaveChanges();
        void Update(T entity);
        public void UpdateRange(IEnumerable<T> entities);
        public Task<T> AddAsync(T entity);
        public GetAllFromSPWithOutputResponseViewModel GetAllFromSPWithOutput(FnSpRequest fnSpRequest);
        public List<Dictionary<string, object>> PosGresFunction(string name, string parameters);
    }

    public class ChatV1Repository<T> : IChatV1Repository<T> where T : class
    {
        private readonly ChatV1Context _context;

        public ChatV1Repository(ChatV1Context context)
        {
            _context = context;
        }

        public T Add(T entity)
        {

            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {

            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public List<T> AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();

            return entities.ToList();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> filter)
        {
            DbSet<T> dbSet1 = _context.Set<T>();
            IQueryable<T> query = dbSet1;

            query = query.Where(filter);
            return query;
        }

        public bool Exists(Expression<Func<T, bool>> filter)
        {
            DbSet<T> dbSet1 = _context.Set<T>();
            IQueryable<T> query = dbSet1;

            bool res = query.Any(filter);
            return res;
        }

        public IQueryable<T> GetAll()
        {
            DbSet<T> dbSet1 = _context.Set<T>();
            IQueryable<T> query = dbSet1;
            return query;
        }

        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();

        }

        public void RemoveById(object id)
        {

            var entity = _context.Set<T>().Find(id);

            _context.Set<T>().Remove(entity);
            _context.SaveChanges();

        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {

            _context.Set<T>().Update(entity);
            _context.SaveChanges();

        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public GetAllFromSPWithOutputResponseViewModel GetAllFromSPWithOutput(FnSpRequest fnSpRequest)
        {

            var baseRepositoryResponseViewModel = new GetAllFromSPWithOutputResponseViewModel();

            Dictionary<string, object> outputs = new Dictionary<string, object>();

            using SqlConnection ErpAviationConn = new SqlConnection(_context.Database.GetConnectionString());
            using var cmd = ErpAviationConn.CreateCommand();

            cmd.CommandText = fnSpRequest.FNSpName;
            cmd.CommandType = CommandType.StoredProcedure;

            string paras = string.Empty;
            for (int i = 0; i < fnSpRequest.Parameters.Count; i++)
            {

                if (fnSpRequest.Parameters[i].Type != null)
                {

                    if (fnSpRequest.Parameters[i].Type.ToLower() == "varbinary")
                    {
                        if (fnSpRequest.Parameters[i].Value == null)
                        {
                            cmd.Parameters.Add($"@{fnSpRequest.Parameters[i].Name}", SqlDbType.VarBinary).Value = System.Data.SqlTypes.SqlBinary.Null;
                        }
                        else
                        {
                            var byteArray = fnSpRequest.Parameters[i].Value.ToString();
                            cmd.Parameters.Add($"@{fnSpRequest.Parameters[i].Name}", SqlDbType.VarBinary, -1).Value = Convert.FromBase64String(byteArray);
                        }
                    }
                }

                else if (fnSpRequest.Parameters[i].Value == null)
                {
                    cmd.Parameters.AddWithValue($"@{fnSpRequest.Parameters[i].Name}", null);
                }

                else
                {
                    cmd.Parameters.AddWithValue($"@{fnSpRequest.Parameters[i].Name}", fnSpRequest.Parameters[i].Value.ToString());

                    if (fnSpRequest.Parameters[i].IsOutPut == true)
                    {
                        cmd.Parameters[$"@{fnSpRequest.Parameters[i].Name}"].Size = 250;
                        cmd.Parameters[$"@{fnSpRequest.Parameters[i].Name}"].Direction = ParameterDirection.Output;
                        outputs.Add(fnSpRequest.Parameters[i].Name, null);
                    }
                }
            }

            DataTable dtres = new DataTable();
            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtres);
            var spTblResult = dtres.ToListOfDictionary();

            foreach (var i in outputs)
            {
                outputs[i.Key] = cmd.Parameters[$"@{i.Key}"].Value;
            }

            baseRepositoryResponseViewModel.OutPuts = outputs;
            baseRepositoryResponseViewModel.Result = spTblResult;

            return baseRepositoryResponseViewModel;


        }

        public List<Dictionary<string, object>> PosGresFunction(string name, string parameters)
        {
            var con = new NpgsqlConnection();
            con.ConnectionString= _context.Database.GetConnectionString();

            NpgsqlCommand cmd =con.CreateCommand();

            cmd.CommandText = $"select * from {name}('{parameters}') ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt.ToListOfDictionary();
        }




    }
}
