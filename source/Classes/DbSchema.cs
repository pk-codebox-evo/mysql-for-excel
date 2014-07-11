﻿// Copyright (c) 2014, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using MySQL.Utility.Classes.MySQLWorkbench;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a MySQL Schema that MySQL for Excel can interact with.
  /// </summary>
  public class DbSchema : DbObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DbSchema"/> class.
    /// </summary>
    /// <param name="connection">The MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="name">The name of the MySQL database object.</param>
    public DbSchema(MySqlWorkbenchConnection connection, string name)
      : base(connection, name)
    {
    }
  }
}
