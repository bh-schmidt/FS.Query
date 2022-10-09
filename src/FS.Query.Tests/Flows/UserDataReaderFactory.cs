using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FS.Query.Tests.Flows
{
    public static class UserDataReaderFactory
    {
        public static IDataReader Fact(int count, string[] columns)
        {
            var dataTable = new DataTable();
            var columnsCount = 0;

            if (columns.Contains("Id"))
            {
                dataTable.Columns.Add("Id", typeof(long));
                columnsCount++;
            }

            if (columns.Contains("Active"))
            {
                dataTable.Columns.Add("Active", typeof(bool));
                columnsCount++;
            }

            if (columns.Contains("Name"))
            {
                dataTable.Columns.Add("Name", typeof(string));
                columnsCount++;
            }

            if (columns.Contains("BirthDay"))
            {
                dataTable.Columns.Add("BirthDay", typeof(DateTime));
                columnsCount++;
            }

            if (columns.Contains("UserPost.Id"))
            {
                dataTable.Columns.Add("Id", typeof(long));
                columnsCount++;
            }

            if (columns.Contains("UserPost.Post"))
            {
                dataTable.Columns.Add("Post", typeof(string));
                columnsCount++;
            }

            if (columns.Contains("UserPost.UserId"))
            {
                dataTable.Columns.Add("User_Id", typeof(long));
                columnsCount++;
            }

            int i = 0;
            while (i < count)
            {
                i++;
                var objects = new List<object>(columnsCount);

                if (columns.Contains("Id"))
                    objects.Add(i);

                if (columns.Contains("Active"))
                    objects.Add(true);

                if (columns.Contains("Name"))
                    objects.Add($"User {i}");

                if (columns.Contains("BirthDay"))
                    objects.Add(DateTime.Now);

                if (columns.Contains("UserPost.Id"))
                    objects.Add(i);

                if (columns.Contains("UserPost.Post"))
                    objects.Add($"Post {i}");

                if (columns.Contains("UserPost.UserId"))
                    objects.Add(i);

                dataTable.Rows.Add(objects.ToArray());
            }

            return dataTable.CreateDataReader();
        }
    }
}
