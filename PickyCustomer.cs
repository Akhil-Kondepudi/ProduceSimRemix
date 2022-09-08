//Author: Akhil Kondepudi
//Date: 1/15/2022
//Revision History: Creation finished on 1/15/2022 by Akhil Kondepudi
using System;

/*Class Invariants:
  This class was created as a certain variation of a specific type of customer. It inherits from the general customer class and 
  represnets a customer that has restrictions on some produce that they want. For example, the customer might have it so that it will be unable to get apples
  and pears because they are alergic to them. Only 5 restrictions can be stored at a given time, and the oldest one is overwritten if more are added
  It retains all the functionallity of the original customer class, so if general questions about the class occur, please refer to
  the parent class for answers.
*/

//Interface Invariants:
// constructor
//  -This is how you create a customer object with all the data the it will need to function
//  -Will set mimnimumPrice equal to balance if it exceeds it
//  -by default there are no exclusions in the class, they must be added via the addExclusion method
// addExclusion
//  -adds exclusion to storage in customer
// isExclusion
//  -takes string of the produce name, and returns bool on whether it is restricted or not.
namespace P3
{
    public class PickyCustomer : Customer
    {
        private string[] exclusions;
        private int exclusionSize;


        //Preconditions: Input all data that you want stored in the customer object
        //Postconditions: inputed data is stored and exclusionslist is set to a length of 5 and only has empty strings
        public PickyCustomer(string Name = "", string Address = "", double Balance = 0, double MinimumPrice = 0, string[] exc = null) : base(Name, Address, Balance, MinimumPrice)
        {
            exc = exc ?? new string[0];
            exclusions = new string[5];
            exclusionSize = 0;
            for (int i = 0; i < exc.Length; i++)
            {
                addExclusion(exc[i]);
            }
        }

        //Preconditions: string must be inputed
        //Postconditions: exclusion is added to list(and replaces oldest value if there are 5 already)
        public void addExclusion(string exc)
        {
            exclusions[exclusionSize % 5] = exc;
            exclusionSize++;
        }

        //Preconditions: produce must be inputed
        //Postconditions: None
        public override bool checkItem(Produce p)
        {
            string name = p.info()[0];
            for (int i = 0; i < exclusions.Length; i++)
            {
                if (exclusions[i] == name)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

/*Implementation Invariants
  Public Functions:
  Constructer
  - the constructor has default values  for error checking, all code will work with default values but will be largely arbitrary
  - exclusionlist is set to a length of 5 with only empty strings because only five values should be stored. The empty strings are so that 
    the length of it can be parsed without throwing errors
  addExclusion
  -string inputed is added to exclusion list. modulues of size is used to garuntee size and also to replace oldest value
  isExclusion
  -parses the exclusionlist to check if inputed value is in it, returns true or false depending on that.
*/