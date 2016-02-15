
import unittest
import packages1.person as person

class CustomObjSampleTest(unittest.TestCase):
    def testEqual(self):
        person1 = person.Person(1,"cheka")
        person2 = person.Person(1,"cheka")
        
        self.assertEqual(person1,person1) # compare with itself
        self.assertTrue(person1 is not person2)
        self.assertEqual(person1,person2)
        self.assertTrue(person1 == person2)
        
        person2._name = "new name"
        self.assertNotEqual(person1,person2)
        self.assertTrue(person1 != person2) # test __ne__
        self.assertNotEqual(1,person1) # test two different types
        
    def testListEqual(self):
        """
        since Person class itself has override "equal" operation
        a collection of person will automatically check equality with its peer based on content other than reference
        """
        list1 = [person.Person(1,"cheka"),person.Person(2,"stasi")]
        list2 = [person.Person(1,"cheka"),person.Person(2,"stasi")]
        self.assertEqual(list1,list2)
        self.assertTrue(list1 == list2)
        self.assertIsNot(list1,list2)
        
    def testDictEqual(self):
        infos = ((1,"cheka"),(2,"stasi"))
        
        dict1 = {}
        dict2 = {}
        for atuple in infos:
            dict1[atuple[0]] = person.Person(atuple[0],atuple[1])
            dict2[atuple[0]] = person.Person(atuple[0],atuple[1])
            
        # ------------ check
        for atuple in infos:
            ssn = atuple[0]
            self.assertTrue(dict1[ssn] is not dict2[ssn])
        self.assertEqual(dict1,dict2)
        
    def testToString(self):
        aperson = person.Person(1,"cheka")
        self.assertEqual("Person<1,cheka>",str(aperson))
        
        
if __name__ == "__main__":
    unittest.main()