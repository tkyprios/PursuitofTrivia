using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia
{

    /// <summary>
    /// Enumeration representing the different categories of trivia questions available in the game. Star Wars themed.
    /// </summary>
    public enum Category
    {
       [Description("Characters")]
        Characters = 1,

       [Description("Planets and Locations")]
        PlanetsAndLocations = 2,

       [Description("Ships, Vehicles and Tech")]
        ShipsVehiclesAndTech = 3,

        EventsAndBattles = 4,


        QuotesAndLore = 5

    }
}
