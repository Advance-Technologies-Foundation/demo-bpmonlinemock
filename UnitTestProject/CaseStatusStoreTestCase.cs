using System;
using System.Collections;
using System.Collections.Generic;
using BL;
using FluentAssertions;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;

namespace UnitTestProject
{

	#region Class: CaseBrockerTestCase

	[MockSettings(RequireMock.DBEngine)]
	[TestFixture]
	public class CaseStatusStoreTestCase : BaseConfigurationTestFixture
	{

		private static readonly Guid ClosedStatusId = Guid.Parse("3E7F420C-F46B-1410-FC9A-0050BA5D6C38");
		private static readonly Guid NewStatusId = Guid.Parse("AE5F2F10-F46B-1410-FD9A-0050BA5D6C38");
		private static readonly Guid WaitStatusId = Guid.Parse("3859C6E7-CBCB-486B-BA53-77808FE6E593");
		private static readonly Guid ResolvedStatusId = Guid.Parse("AE7F411E-F46B-1410-009B-0050BA5D6C38");

		private readonly CaseStatusStore _defaultStore = new CaseStatusStore(GetDefaultStore());


		public class TestData
		{
			public static IEnumerable FinalTestCases {
				get {
					yield return new TestCaseData(ClosedStatusId).Returns(true);
					yield return new TestCaseData(NewStatusId).Returns(false);
				}
			}
			public static IEnumerable PausedTestCases {
				get {
					yield return new TestCaseData(ClosedStatusId).Returns(false);
					yield return new TestCaseData(NewStatusId).Returns(false);
					yield return new TestCaseData(WaitStatusId).Returns(true);
				}
			}
			public static IEnumerable ResolvedTestCases {
				get {
					yield return new TestCaseData(ClosedStatusId).Returns(false);
					yield return new TestCaseData(NewStatusId).Returns(false);
					yield return new TestCaseData(ResolvedStatusId).Returns(true);
				}
			}
		}

		#region Methods: Private

		private static IEnumerable<CaseStatusStore.CaseStatusData> GetDefaultStore() {
			return new List<CaseStatusStore.CaseStatusData> {
				new CaseStatusStore.CaseStatusData {
						Id = ClosedStatusId,
						IsFinal = true,
						Name = "Closed"
				}, new CaseStatusStore.CaseStatusData {
						Id = NewStatusId,
						Name = "New"
				}, new CaseStatusStore.CaseStatusData {
						Id = WaitStatusId,
						IsPaused = true,
						Name = "Waiting for response"
				}, new CaseStatusStore.CaseStatusData {
						Id = ResolvedStatusId,
						IsResolved = true,
						Name = "Resolved"
				}

			};
		}

		#endregion

		#region Methods: Protected

		protected override IEnumerable<Type> GetRequiringInitializationSchemas() {
			EntitySchemaManager.AddCustomizedEntitySchema("CaseStatus", new Dictionary<string, string> {
				{ "Name", "ShortText" },
				{ "IsFinal", "Boolean" },
				{ "IsResolved", "Boolean" },
				{ "IsPaused", "Boolean" }

			});
			return base.GetRequiringInitializationSchemas();
		}

		#endregion

		#region Methods: Public
		[TestCaseSource(typeof(TestData), nameof(TestData.FinalTestCases))]
		[Test, Category("PreCommit")]
		public bool CaseStatusStore_CheckIsStatusFinalById(Guid statusId) {
			return _defaultStore.StatusIsFinal(statusId);
		}

		[TestCaseSource(typeof(TestData), nameof(TestData.PausedTestCases))]
		[Test, Category("PreCommit")]
		public bool CaseStatusStore_CheckIsStatusPausedByIds(Guid statusId) {
			return _defaultStore.StatusIsPaused(statusId);
		}

		[TestCaseSource(typeof(TestData), nameof(TestData.ResolvedTestCases))]
		[Test, Category("PreCommit")]
		public bool CaseStatusStore_CheckIsStatusResolvedByIds(Guid statusId) {
			return _defaultStore.StatusIsResolved(statusId);
		}

		[TestCaseSource(typeof(TestData), nameof(TestData.ResolvedTestCases))]
		[Test, Category("PreCommit")]
		public bool CaseStatusStore_CheckIsStatusResolvedByIds_CallLoadData(Guid statusId) {
			new SelectData(UserConnection, "CaseStatus")
				.AddRow(new Dictionary<string, object> { { "Id", ClosedStatusId }, { "Name", "Closed" }, { "IsFinal", true }, { "IsPaused", false }, { "IsResolved", false } })
				.AddRow(new Dictionary<string, object> { { "Id", NewStatusId }, { "Name", "New" }, { "IsFinal", false }, { "IsPaused", false }, { "IsResolved", false } })
				.AddRow(new Dictionary<string, object> { { "Id", ResolvedStatusId }, { "Name", "Resolved" }, { "IsFinal", false }, { "IsPaused", false }, { "IsResolved", true } })
				.MockUp();
			var store = new CaseStatusStore(UserConnection);
			return store.StatusIsResolved(statusId);
		}

		[Test, Category("PreCommit")]
		public void CaseStatusStore_CheckIsStatusResolvedByIds_StatusIsGuidEmpty() {
			_defaultStore.StatusIsFinal(Guid.Empty).Should().BeFalse();
			_defaultStore.StatusIsPaused(Guid.Empty).Should().BeFalse();
			_defaultStore.StatusIsResolved(Guid.Empty).Should().BeFalse();
		}

		#endregion

	}

	#endregion
}
