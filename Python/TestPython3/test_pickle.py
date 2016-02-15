
import unittest
from packages1.person import Person
import pickle

class PickleTest(unittest.TestCase):
    
    def test_dumps(self):
        p0 = Person(9,"stasi")
        byteobj = pickle.dumps(p0)
        
        p_copy = pickle.loads(byteobj)
        self.assertIsInstance(p_copy,Person)
        self.assertTrue(p0 is not p_copy)
        self.assertEqual(p0,p_copy)