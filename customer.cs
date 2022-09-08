//Author: Akhil Kondepudi
//Date: 2/14/2022

//Interface Invariants:
// Customer constructor
//  -This is how you create a customer object with all the data the it will need to function
//  -Will set mimnimumPrice equal to balance if it exceeds it
// info
//  -returns a string vector that contains the basic data about the customer to the client
// addProduce
//  -adds the inputed produce object to a vector that the customer holds
// changeBalance
//  -changes balance based on the inputed double

/*Class Invariants:
This class was created as a way for a delivery service to keep track of their customers
Each customer object basically acts as a virtual representation of the customer, and will basic information
about the customer like their name, and how much money they have in their account. The client can query for
information about the customer but as of right now, they can only modify the balance. The constructor allows for
the construction of a customer with no information stored in it, but it is HIGHLY ADIVESED that the client would
fill out these fields when constructing the object
*/

using System;


namespace P3
{
    public class Customer
    {
        private string name;
        private string address;
        private double balance;
        private double minimumPrice;
        private Produce[] produceItems;
        private int maxSize;
        private int size;


        // Preconditions: None, Although input of fields is highly advised
        // Postconditions: fields are initialized with data proveided, or empty strings and 0s if no data is provided
        public Customer(string Name = "", string Address = "", double Balance = 0, double MinimumPrice = 0) 
        { 
            name = Name;
            address = Address;
            balance = Math.Round(Balance, 2);
            minimumPrice = Math.Round(MinimumPrice, 2);
            produceItems = new Produce[5];
            maxSize = 5;
            size = 0;
        }

        // Preconditions: None
        // Postconditions: A string array about information of the customer is returned
        public string[] info()
        {
            string[] output = new string[4];
            output[0] = name;
            output[1] = address;
            output[2] = Math.Round(balance, 2).ToString();
            output[3] = Math.Round(minimumPrice, 2).ToString();
            return output;
        }

        // Preconditions: Produce object must be inputed
        // Postconditions: Produce object is added toe the clients posession
        public void addProduce(Produce p)
        {
            if (size == maxSize)
            {
                increaseSize();
            }
            produceItems[size] = p;
            size++;

        }

        // Preconditions: valid double is inputed
        // Postconditions: inputed double is added to the balance, whether negative or positive
        public virtual void changeBalance(double change)
        {
            balance += Math.Round(change, 2);
            balance = balance < 0 ? 0 : balance;
        }
        
        // Preconditions: None
        // Postconditions: maxsize increases and array size is also increased by 5
        private void increaseSize()
        {
            maxSize += 5;
            Array.Resize(ref produceItems, maxSize);
        }

        public virtual bool checkItem(Produce p) {
            return true;
        }

    }
}

/*Implementation Invariants
  Public Functions:
  Constructer
  - the constructor has default values  for error checking, all code will work with default values but will be largely arbitrary
  info
  -simply returns a sting array that contains all the data in the fields
  addProduce
  -adds the inputed produce item to produceItems vector
  changeBalance
  -adds value to balance member
  increaseSize
  -increases size of array using array class functionality, also increases max size variable to refelect that 
*/
