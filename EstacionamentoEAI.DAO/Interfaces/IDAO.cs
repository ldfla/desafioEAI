using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace EstacionamentoEAI.DAO.Interfaces
{
    public interface IDAO<T>:IDisposable 
        where T: class, new()
    {
        /// <summary>
        /// Metodo para inserir dados no banco de dados
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        T Inserir(T model);

        /// <summary>
        /// Metodo para atualizar um registro no banco de dados
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Atualizar(T model);

        /// <summary>
        /// Metodo para realizar buscar no banco de dados a partir de um parametro
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        T BuscarItem(params Object[] objeto);

        /// <summary>
        /// Metodo para recuperar todos os items de um objeto <T>
        /// </summary>
        /// <returns></returns>
        List<T> ListarItens();
    }
}
