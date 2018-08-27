using System;
using System.Collections.Generic;
using System.Linq;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Store;

namespace BL
{

	#region Class: CaseStatusStore

	/// <summary>
	/// Class represents storage of case statuses.
	/// </summary>
	public class CaseStatusStore
	{

		public static readonly Guid CaseStatusReopenedId = new Guid("F063EBBE-FDC6-4982-8431-D8CFA52FEDCF");

		#region Class: CaseStatusData

		/// <summary>
		/// Class represents case status data from data base.
		/// </summary>
		public class CaseStatusData
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
			public bool IsFinal { get; set; }
			public bool IsResolved { get; set; }
			public bool IsPaused { get; set; }
		}

		#endregion

		#region Constructor: Public

		/// <summary>
		/// Creates new <see cref="CaseStatusStore"/> instance, using <paramref name="userConnection"/> 
		/// for loading data from data base.
		/// </summary>
		/// <param name="userConnection">User connection <see cref="UserConnection"/> instance.</param>
		public CaseStatusStore(UserConnection userConnection) {
			UserConnection = userConnection;
		}

		/// <summary>
		/// Creates new <see cref="CaseStatusStore"/> instance, using prepared data store <paramref name="store"/>.
		/// </summary>
		/// <param name="store">Prepared data store.</param>
		public CaseStatusStore(IEnumerable<CaseStatusData> store) {
			_caseStatusStoreData = store;
		}

		#endregion

		#region Properties: Private

		private IEnumerable<CaseStatusData> _caseStatusStoreData;
		private IEnumerable<CaseStatusData> CaseStatusStoreData => _caseStatusStoreData ?? (_caseStatusStoreData = LoadData());

		#endregion

		#region Properties: Protected

		/// <summary>
		/// Instance of user connection.
		/// </summary>
		protected UserConnection UserConnection { get; }

		#endregion

		#region Methods: Private

		private EntitySchemaQuery GetEsqCaseStatus() {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "CaseStatus");
			esq.PrimaryQueryColumn.IsAlwaysSelect = true;
			esq.AddColumn("Name");
			esq.AddColumn("IsFinal");
			esq.AddColumn("IsResolved");
			esq.AddColumn("IsPaused");
			esq.Cache = UserConnection.SessionCache.WithLocalCaching("CaseStatusStoreCache");
			esq.CacheItemName = "CaseStatusStoreCacheItem";
			return esq;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Load case status data from data base.
		/// </summary>
		/// <returns>
		/// Collection case status data.
		/// </returns>
		protected IEnumerable<CaseStatusData> LoadData() {
			var esq = GetEsqCaseStatus();
			var entities = esq.GetEntityCollection(UserConnection);
			return entities.Select(entity => new CaseStatusData {
				Id = entity.GetTypedColumnValue<Guid>(esq.RootSchema.GetPrimaryColumnName()),
				Name = entity.GetTypedColumnValue<string>("Name"),
				IsFinal = entity.GetTypedColumnValue<bool>("IsFinal"),
				IsPaused = entity.GetTypedColumnValue<bool>("IsPaused"),
				IsResolved = entity.GetTypedColumnValue<bool>("IsResolved")
			});
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Check if case status is marked as final. Returns true if it is final, otherwise false.
		/// </summary>
		/// <param name="statusId">Uniqueidentifier of case status.</param>
		/// <returns>
		/// Checks is case status final.
		/// </returns>
		public bool StatusIsFinal(Guid statusId) {
			return CaseStatusStoreData
				.Find(status => status.Id == statusId)?.IsFinal ?? false;
		}

		/// <summary>
		/// Check if case status is marked as paused. Returns true if it is paused, otherwise false.
		/// </summary>
		/// <param name="statusId">Uniqueidentifier of case status.</param>
		/// <returns>
		/// Checks is case status paused.
		/// </returns>
		public bool StatusIsPaused(Guid statusId) {
			return CaseStatusStoreData
				.Find(status => status.Id == statusId)?.IsPaused ?? false;
		}

		/// <summary>
		/// Check if case status is marked as resolved. Returns true if it is resolved, otherwise false.
		/// </summary>
		/// <param name="statusId">Uniqueidentifier of case status.</param>
		/// <returns>
		/// Checks is case status resolved.
		/// </returns>
		public bool StatusIsResolved(Guid statusId) {
			return CaseStatusStoreData
				.Find(status => status.Id == statusId)?.IsResolved ?? false;
		}

		/// <summary>
		/// Check if case status is marked as reopened. Returns true if it is reopened, otherwise false.
		/// </summary>
		/// <param name="statusId">Uniqueidentifier of case status.</param>
		/// <returns>
		/// Checks is case status reopened.
		/// </returns>
		public bool StatusIsReopened(Guid statusId) {
			return statusId == CaseStatusReopenedId;
		}

		#endregion

	}

	#endregion

}