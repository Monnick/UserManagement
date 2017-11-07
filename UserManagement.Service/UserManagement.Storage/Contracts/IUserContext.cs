﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserManagement.Storage.Contracts
{
    public interface IUserContext
	{
		IQueryable<Entities.User> Users { get; }

		int SaveChanges();

		void Update(Entities.User user);

		void Add(Entities.User user);

		Entities.User Find(Guid? id);
	}
}
