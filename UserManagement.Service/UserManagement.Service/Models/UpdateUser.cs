using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Service.Models
{
    public class UpdateUser
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public string Street { get; set; }

		public string City { get; set; }

		public string Zip { get; set; }

		public string Country { get; set; }
		
		public string Password { get; set; }

		public string OldPassword { get; set; }
	}
}
