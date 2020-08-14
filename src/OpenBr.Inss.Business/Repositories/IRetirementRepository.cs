﻿using OpenBr.Inss.Business.Documents;
using OpenBr.Inss.Business.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Repositories
{

    /// <summary>
    /// Retirement contribution repository interface
    /// </summary>
    public interface IRetirementRepository : IRepository<Retirement>
    {

        /// <summary>
        /// Get active retirement 
        /// </summary>
        /// <param name="type">Retirement type</param>
        /// <param name="cancellationToken">Operation cancellation token</param>
        Task<Retirement> GetActive(RetirementType type, CancellationToken cancellationToken = default);

    }

}