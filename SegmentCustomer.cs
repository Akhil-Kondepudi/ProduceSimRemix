//Author: Akhil Kondepudi
//Date: 1/15/2022
//Revision History: Creation finished on 1/15/2022 by Akhil Kondepudi
using System;

/*Class Invariants:
  This class was created as a certain variation of a specific type of customer. It inherits from the general customer class and 
  represnets a customer that only wants specific classifications of produce. This means that they want ony fruit, or they only want fungus and 
  vegtable produce. It retains all the functionallity of the original customer class, so if general questions about the class occur, please refer to
  the parent class for answers.
*/

//Interface Invariants:
// constructor
//  -This is how you create a customer object with all the data the it will need to function
//  -Will set mimnimumPrice equal to balance if it exceeds it
//  -by default there are no classifications in the class, they must be added via the addClassification method
// addClassification
//  -adds Classification to storage in customer
// isSameClassification
//  -takes string of the produce classification, and returns bool on whether it is included or not.

namespace P3
{
    public class SegmentCustomer : Customer
    {
        private string[] includeClassifications;
        private int arrSize;

        //Preconditions: Input all data that you want stored in the customer object
        //Postconditions: inputed data is stored and includeClassifications is set to empty array
        public SegmentCustomer(string Name = "", string Address = "", double Balance = 0, double MinimumPrice = 0, string[] classifications = null) : base(Name, Address, Balance, MinimumPrice)
        {
            classifications = classifications ?? new string[0];
            includeClassifications = new string[5];
            arrSize = 0;
            for (int i = 0; i < classifications.Length; i++)
            {
                addClassification(classifications[i]);
            }
        }

        //Preconditions: string must be inputed
        //Postconditions: string is added to includeClassifications, if it is not big enough, its size is increased
        public void addClassification(string clasif)
        {
            if (arrSize == includeClassifications.Length)
            {
                Array.Resize(ref includeClassifications, arrSize + 5);
            }
            includeClassifications[arrSize] = clasif;
            arrSize++;
        }

        //Preconditions: produce is inputed
        //Postconditions: none
        public override bool checkItem(Produce p)
        {
            string clasif = p.info()[4];
            for (int i = 0; i < arrSize; i++)
            {
                if (clasif == includeClassifications[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}

/*Implementation Invariants
  Public Functions:
  Constructer
  - the constructor has default values  for error checking, all code will work with default values but will be largely arbitrary
  - Classification list has size set to 5, but this is arbitrary as size can be increased when strings are added
  addClassification
  -string inputed is added to exclusion list. increases size of array if the size is not big enough
  isSameClassification
  -parses the includeClassification array to check if inputed value is in it, returns true or false depending on that.
*/