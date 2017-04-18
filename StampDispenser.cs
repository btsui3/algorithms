using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Stamps
{

    /*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
      
	  Summary : StampDispenser facilitates dispensing stamps for a postage stamp machine.    
	
	The StampDispenser class represents a postage stamp vending machine.  The machine contains stamps of different values. 
	The machine will always have a stamp with a value of 1 cent and the machine will never run out of any type of stamp. 
	The machine should allow someone to calculate the minimum number of stamps that the machine can dispense to fill a given request. 
	The code should work for countries with high denomination values where stamp values of 1000 or 9000 are common. 

	
    %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */


    public class StampDispenser
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //  Make "stampArray" available in all methods in StampDispenser class.
        private int[] stampArray { get; set; }
        private int[] tempStampDenominations { get; set;}
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //Method for array input validation.
        public StampDispenser(int[] stampDenominations)
        {


            // Check if input array is empty
            if (stampDenominations.Length == 0)
            {
                throw new System.ArgumentException("Parameter must contain at least one stamp value");
            }


            // Throw error if array contains value less than or equal to zero
            foreach (int stampValue in stampDenominations)
            {
                if (stampValue <= 0)
                {
                    throw new System.ArgumentException("Parameter cannot contain values equal or less than zero");
                }
            }


            // Confirm array is numerically sorted in descending order
            Array.Sort(stampDenominations);
            Array.Reverse(stampDenominations);


            //Confirm there are no duplicate values in input array
             tempStampDenominations = stampDenominations.Distinct().ToArray();


            // Check if input array contains value of 1 
            if (stampDenominations[stampDenominations.Length - 1] != 1)
            {
                List<int> stampList = new List<int>();

                foreach (int stampValue in stampDenominations)
                {
                    stampList.Add(stampValue);
                    
                }
                stampList.Add(1);
                stampArray = stampList.ToArray();

            }
           


            // Store validated input array into "stampArray", which is used in the CalcMinNumStampsToFillRequest method
            //stampArray = tempStampDenominations;
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        // Input =>  Total value of stamps to be dispensed.  
        // Output => Minimum number of stamps needed to fill the given request.
        // For more information on algorithm, see http://www.columbia.edu/~cs2035/courses/csor4231.F07/dynamic.pdf 

        public int CalcMinNumStampsToFillRequest(int request)
        {
            // stampTable holds the min number of stamps for each value up to "request", so stampTable[request] = min num of stamps
            int[] stampTable = new int[(request + 1)];


            // Base case (If given request is 0)
            stampTable[0] = 0;


            // Initialize all stampTable values as MAX to allow for high values and avoid overflow
            for (int i = 1; i <= request; i++)
            {
                stampTable[i] = int.MaxValue;
            }


            // Compute min stamps required for all values from 1 to 'request'
            for (int stampItem = 1; stampItem <= request; stampItem++)
            {
                //Go through all stamps smaller than currentStamp
                foreach (int currentStamp in stampArray)
                {
                    // Consider stamps less than current value
                    if (currentStamp <= stampItem)
                    {
                        //Subtracting the currentStamp from stampItem allows the reuse of existing sub-results, this is memoization( https://en.wikipedia.org/wiki/Memoization )
                        int subResult = stampTable[stampItem - currentStamp];

                        // If the number of stamps is less than what is in the table's currrent value, set minimum stamp result                     
                        if (subResult != int.MaxValue && subResult + 1 < stampTable[stampItem])
                        {
                            stampTable[stampItem] = subResult + 1;
                        }
                    }
                }
            }
            return stampTable[request];
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        class Program
        {
            static void Main(string[] args)
            {
                // Test LaserFicheExample

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
                StampDispenser stampDispenserGivenExample = new StampDispenser(new int[] { 90, 30, 24, 10, 6, 2, 1 });
                Debug.Assert(stampDispenserGivenExample.CalcMinNumStampsToFillRequest(18) == 3);
                Debug.Assert(stampDispenserGivenExample.CalcMinNumStampsToFillRequest(34) == 2);
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//




                // Test Request of One

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
                StampDispenser stampDispenserTestWithout1 = new StampDispenser(new int[] { 90, 30, 24, 10, 6, 2});
                Debug.Assert(stampDispenserTestWithout1.CalcMinNumStampsToFillRequest(18) == 3);
                Debug.Assert(stampDispenserTestWithout1.CalcMinNumStampsToFillRequest(34) == 2);
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//





                //// Test Request of Zero

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
                StampDispenser stampDispenserTestZero = new StampDispenser(new int[] { 90, 30, 24, 10, 6, 2, 1 });
                Debug.Assert(stampDispenserTestZero.CalcMinNumStampsToFillRequest(0) == 0);
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//




                // Test for removal of duplicates

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
                StampDispenser stampDispenserDuplicates = new StampDispenser(new int[] { 90, 90, 90, 30, 24, 10, 6, 2, 1 });
                Debug.Assert(stampDispenserDuplicates.CalcMinNumStampsToFillRequest(18) == 3);
                Debug.Assert(stampDispenserDuplicates.CalcMinNumStampsToFillRequest(34) == 2);
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


                // Test for sorted array in descending order

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
                StampDispenser stampDispenserUnsortedArray = new StampDispenser(new int[] { 1, 5, 12, 25 });
                Debug.Assert(stampDispenserUnsortedArray.CalcMinNumStampsToFillRequest(16) == 4);
                Debug.Assert(stampDispenserUnsortedArray.CalcMinNumStampsToFillRequest(31) == 3);
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


                // Test for high values (different currencies)

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
                StampDispenser stampDispenserLargeNumberTest = new StampDispenser(new int[] { 1000000, 500000, 100000, 50000, 10000, 9000, 8000, 7000, 5000, 3000, 1000, 700, 500, 260, 100, 50, 12, 10, 2, 1 });
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(57) == 5);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(143) == 6);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(760) == 2);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(1740) == 5);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(12001) == 3);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(58113) == 5);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(77787) == 10);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(128001) == 5);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(487647) == 16);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(177787) == 11);
                Debug.Assert(stampDispenserLargeNumberTest.CalcMinNumStampsToFillRequest(100000000) == 100);

                Console.WriteLine("Passed all Tests");
                Console.ReadKey();
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
            }
        }
    }   //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
}

