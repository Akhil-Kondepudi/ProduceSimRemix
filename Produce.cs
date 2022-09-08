//Author: Akhil Kondepudi
//Date: 1/12/2022
//Revision History: Creation finished on 1/12/2022 by Akhil Kondepudi

using System;

//Class Invariants:
//This class was created as a way for grocery stores to keep track of their produce items.
//After initilizing an instance of the produce object with appropriate data, the object should be able to act as a virtual
//representation of the actual produce item. Using this object, the client shoulf be able to efficiantly do things like manage
//their inventory and check on the quality of the produce item (like if it is spoiled). The client can easily change the quantity
//or the cost of the item in case of senarios like one quantity being sold or the price changing due to a sale. The class will 
//by default update data using system time so functions can be called efficiently and easily. This class will work effectlively only
//if preconditions for constructor and methods are strictly followed. If not, the data will likeely be faulty or errors will be thrown.
//Due to the intended usage of this class, there is no default constructor available, as an empty produce object is nonsensical in the 
//context of an actual grocery store.


//Interface Invariants:
// Produce constructor
//  -This is how you create a produce object with all the data the it will need to function
//  -This will only work if the Preconditions are followed explicitly
//  -does not have safety checks for illogical inputs(like negative quantity). It is up to client to proplerly input if they want proper utility
// Delivery copy constructor
//  -Creates a delivery object with the exact same data as the inputed delivery object
//  -Only works if a valid delivery object is inputed
// checkSpoiled
//  -This is how to check if the current produce item is spoiled, which occurs if the current time is passed the expiration date
//  -This does allow for an input of a custom time. HOWEVER, this custom time is only intended for class testing. It is not advised for client use
//  -Keep note that once an item is spoiled, it is impossible for it to be marked as unspoiled ever again
// setSpoiled
//  -This is for if the client would like to manually set the item as spoiled for unforseen reasons
// checkExpired
//  -This is how to check if the current produce item is expired, which occurs based on improper storing conditions for certain lengths of time
//  -This does allow for an input of a custom time. HOWEVER, this custom time is only intended for class testing. It is not advised for client use
//  -Keep note that once an item is expired, it is impossible for it to be marked as not expired ever again\
// moveStorage
//  -Will change the storage location to new inputed location, and by default will change storage time to system time
//  -This does allow for an input of a custom time. HOWEVER, this custom time is only intended for class testing. It is not advised for client use
//  -Will check if the item will have expired before moving locations due to improper storing conditions.
// blackoutOccur
//  -Should be called on item in event of blackout in order to determine if item is expired because of it.
// info
//  -returns a vector of strings that contains the basic info about the produce object
// changeAmount
//  -setter for the amount field
// changeCost
//  -setter for the cost field

namespace P3
{
    public class Produce
    {
        private int quantity;
        private double cost;
        private string name;
        private string unit;
        private string classification;
        private string storage;
        private time storageDate;
        private string storageRec;
        private time expireDate;
        private bool spoiled;
        private bool expired;
        private struct time
        {
            public int year;
            public int month;
            public int day;
            public int hour;
            public int minute;
        }

        //Preconditions: All the parameters must be inputed in the proper format. Specifically:
        // the name of the produce item as a string
        // the amount of the item 
        // the cost of one unit of the item as double. (if one unit costs $3.50, then input 3.50)
        // the units the item is being stored as, like oz or lb. (if the unit is the item itself, input "cnt")
        // the classification of the item, specifically whether it is a "fruit", "vegetable", "fungus", or "herb"
        // the location the item is currently being stored, specifically in either "refrigerator", "dark" (as in dark place), or "counter" (as in counter top)
        // the date and time in which the item started being stored at the given location, given in the format of "month-date-year-military time" all as numbers
        // the location that the item should be stored in, specifically in either "refrigerator", "dark" (as in dark place), or "counter" (as in counter top)
        // the expiration date of the item, given in the format of "month-date-year-military time" all as numbers
        //Postconditions: A produce  object will be initialized that holds all the data that has been inputed
        public Produce(string Name, int Qty, double Cost, string Unit, string classific, string store, string harvDate, string storeRec, string expireDt)
        {
            name = Name;
            quantity = Qty;
            cost = Math.Round(Cost, 2);
            unit = Unit;
            classification = classific;
            storage = store;
            storageDate = convertTime(harvDate);
            storageRec = storeRec;
            expireDate = convertTime(expireDt);
            spoiled = false;
            expired = false;
        }

        // Preconditions: Valid delivery object must be inputed
        // Postconditions: Current object becomes a copy of the inputed delivery object
        public Produce(Produce copy)
        {
            name = copy.name;
            quantity = copy.quantity;
            cost = copy.cost;
            unit = copy.unit;
            classification = copy.classification;
            storage = copy.storage;
            storageDate = copy.storageDate;
            storageRec = copy.storageRec;
            expireDate = copy.expireDate;
            spoiled = copy.spoiled;
            expired = copy.expired;

        }

        //Precondition: If customized current time is wanted, input time in format of "month-date-year-military time" all as numbers.
        //Otherwise, will default to the system time.
        //Postcondition: marks item as spoiled if should be marked as spoiled, and returns true if item is spoiled
        public bool checkSpoiled(string tempTime = "")
        {
            if (spoiled)
            {
                return spoiled;
            }
            time currTime = tempTime == "" ? currentTime() : convertTime(tempTime);
            spoiled = compareTime(expireDate, currTime) < 0;
            return spoiled;
        }

        //Precondition: None
        //Postcondition: will set spoiled to true, esentiallly marking the item as spoiled
        public void setSpoiled()
        {
            spoiled = true;
        }

        //Precondition: If customized current time is wanted, input time in format of "month-date-year-military time" all as numbers.
        //Otherwise, will default to the system time.
        //Postcondition: marks item as expired if should be marked as expired, and returns true if item is expired
        public bool checkExpired(string tempTime = "")
        {
            time currTime = tempTime == "" ? currentTime() : convertTime(tempTime);
            int timePassed = compareTime(currTime, storageDate);
            if (timePassed > 60 && storage != "refrigerator" && storageRec == "refrigerator")
            {
                expired = true;
            }
            if (timePassed > 1440 && storage != "dark" && storageRec == "dark")
            {
                expired = true;
            }
            return expired;
        }

        //Precondition: Input the storage location that the item is being moved to, specifically in either "refrigerator",
        //"dark" (as in dark place), or "counter" (as in counter top).
        //If customized current time is wanted, input time in format of "month-date-year-military time" all as numbers.
        //Otherwise, will default to the system time.
        //Postcondition: will set expired to true if appropriate, and change storage and storageDate as given
        public void moveStorage(string location, string tempTime = "")
        {
            time currTime = tempTime == "" ? currentTime() : convertTime(tempTime);
            checkExpired(tempTime);
            storage = location;
            storageDate = currTime;
        }

        //Precondition: None
        //Postcondition: will set expired to true if appropriate (if storage requirement is "refrigerator")
        public void blackoutOccur()
        {
            expired = storageRec == "refrigerator" ? true : expired;
        }

        //Precondition: None
        //Postcondition: Simply returns a array of strings that has info of the Produce item in the order of:
        //               "name, quantity, unit, cost, classification, storage location"
        public string[] info()
        {
            string[] output = new string[6];
            output[0] = name;
            output[1] = quantity.ToString();
            output[2] = unit;
            output[3] = cost.ToString();
            output[4] = classification;
            output[5] = storage;
            return output;

        }

        //Precondition: proper intger must be inputed
        //Postcondition: sets amount equal to the inputed integer if int is valid
        public void changeAmount(int newAmnt)
        {
            quantity = newAmnt > 0 ? newAmnt : quantity;
        }

        //Precondition: proper double must be inputed
        //Postccndition: sets the cost equal to unputed double if double is valid
        public void changeCost(double newCost)
        {
            cost = newCost > 0 ? Math.Round(newCost, 2) : cost;
        }

        //Precondition: time is given in format of "month-date-year-military time" all as numbers
        //Postcondition: will return struct of time with data based on input
        private time convertTime(string date)
        {
            string[] times = date.Split('-');
            time temp = new time();
            temp.month = Int32.Parse(times[0]);
            temp.day = Int32.Parse(times[1]);
            temp.year = Int32.Parse(times[2]);
            temp.hour = Int32.Parse(times[3].Substring(0, 2));
            temp.minute = Int32.Parse(times[3].Substring(2));
            return temp;
        }

        //Precondition: two filled out time structs must be inputed (no empty fields)
        //Postcondition: will return time difference between the inputed times in minutes
        private int compareTime(time first, time second)
        {
            int[] daysInMonth = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            int minuteDiff = (first.year - second.year) * 525600;
            int firstDays = daysInMonth[first.month - 1] + first.day;
            int secondDays = daysInMonth[second.month - 1] + second.day;
            minuteDiff += (firstDays - secondDays) * 1440;
            minuteDiff += (first.hour * 60 + first.minute) - (second.hour * 60 + second.minute);
            return minuteDiff;
        }

        //Precondition: None
        //Postcondition: Returns time struct based on current system time.
        private time currentTime()
        {
            DateTime now = DateTime.Now;
            time temp = new time();
            temp.day = now.Day;
            temp.month = now.Month;
            temp.year = now.Year;
            temp.hour = now.Hour;
            temp.minute = now.Minute;
            return temp;
        }
    }
}

//Implementation Invariants
//Public functions
// checkSpoiled
//  -I first check if the spoiled field is marked as true, because if it is then going through the rest of the function is
//   not only a waste of time, but has the posibility of setting spoiled from true to false which should be impossible
//  -if spoiled is false, then the code checks if it should be true based on the time difference using the compareTime private method
//   to compare the current time to the expiration date
// checkExpired
//  -first I establish what the function should be using as the current time and compare that with the storage time using the 
//   compareTime private method to figure out how long the item has been stored at its current location
//  -I used if statements considering the time passed, the current storage location, and the location the item should be stored,
//   in order to determine if the item should be expired based on the given rules for expiration.
//  -method is designed so that expired can be set to true, but once set to true can never become false again.
// moveStorage
//  -Uses checkExpired to check if item is expired before moving locations because expiration is storage dependant
//  -only partially complete as it needs functionality to accomodate expiration if moving from bewtween two places that both don't match storage requirement
// blackoutOccur
//  -a simple bit of code built to accomodate blackouts to user specification
//  -current specification is that all items that require refrigeration are immediately expired
// info
//  -simply makes an array with that contains all data avaliable tp client in the form of array for easy access
// changeAmount
//  -sets the value of quantity if inputed value is positive, else it doesnt change the value
// changeCost
//  -sets the value of cost if inputed value is positive, else it doesnt change the value
//  -does not nead to check for sigfig on the double as the info function already accounts for it
//
//Private functions
// convertTime
//  -converts a string structured in the specific format of  "month-date-year-military time" in numbers to the time struct for time comparisons
//  -works using string manipulation so inputed string must be in that format to work
// compareTime
//  -works essentailly by converting each time field value to its minute equivilant and then subtracting based on that value
//  -due to implementation, does not account for leap years. funtionality for leap years does not need to be added for another 2 years.
// currentTime
//  -finds the current time of the system using the DateTime object and then extracts values from that object to create a time struct with that data