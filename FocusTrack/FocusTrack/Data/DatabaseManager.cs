using System;
using System.Collections.Generic;
using System.Linq;
using FocusTrack.Models;

namespace FocusTrack.Data
{
    public class DatabaseManager
    {
        
        public void SaveSession(AppSession newSession)
        {
            using (var db = new AppDbContext())
            {
                db.AppSessions.Add(newSession);
                db.SaveChanges();
            }
        }

        
        public List<AppSession> GetAllSessions()
        {
            using (var db = new AppDbContext())
            {
                return db.AppSessions.ToList();
            }
        }
    }
}