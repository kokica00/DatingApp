using System;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static int CalculateAge(this DateTime theDateTime) 
        {
            var Age = DateTime.Today.Year - theDateTime.Year;
            if(theDateTime.AddYears(Age) > DateTime.Today) 
                Age--;
            
            return Age;
        }
    }
}