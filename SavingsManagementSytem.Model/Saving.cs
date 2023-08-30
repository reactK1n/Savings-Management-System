using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsManagementSystem.Model
{
	public class Saving : BaseEntity
	{
		[ForeignKey(nameof(Member))]
		public string MemberId { get; set; }

		public decimal AmountContribute { get; set; }

		public decimal TotalContribution { get; set; }

		//navigation properties

		public Member Member { get; set; }
	}
}
