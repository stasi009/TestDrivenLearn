
import unittest
from packages1.person import Person
import cPickle # cPickle can be up to 1000 times faster than pickle because the former is implemented in C
import cStringIO

class PickleTest(unittest.TestCase):
    
    def test_dumps_single(self):
        p0 = Person(9,"stasi")
        byteobj = cPickle.dumps(p0)
        self.assertIsInstance(byteobj,str)# in python2, str contains raw 8-bit (1 byte) values
        
        p_copy = cPickle.loads(byteobj)
        self.assertIsInstance(p_copy,Person)
        self.assertTrue(p0 is not p_copy)
        self.assertEqual(p0,p_copy)

    def test_dumps_multiple(self):
        people = [Person(9,"stasi"),Person(8,"kgb"),Person(6,"gru")]

        # -------------------- write
        output = cStringIO.StringIO()
        for p in people:
            cPickle.dump(p,output)
        contents = output.getvalue()
        output.close()

        # -------------------- read
        input = cStringIO.StringIO(contents)
        loaded = []
        try:
            while True:
                loaded.append(cPickle.load(input))
        except EOFError:
            pass

        # -------------------- read
        self.assertEqual(people,loaded)
        self.assertEqual(["stasi","kgb","gru"],[p._name for p in loaded])
        self.assertEqual([9,8,6],[p._ssn for p in loaded])