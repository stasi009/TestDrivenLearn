
import unittest

class EqualIsTest(unittest.TestCase):

    # ======================================== #
    # helper classes
    # ======================================== #
    class ContentBasedEqualObj(object):
        def __init__(self, a):
            self.a = a

        def __eq__(self,other):
            if other is None:
                return False
            elif self is other:# check whether is the same reference
                return True
            elif isinstance(other,self.__class__):
                return self.a == other.a
            else:
                return False
        
        def __ne__(self,other):
            return not self == other

    class ReferenceBasedEqualObj(object):
        def __init__(self,a):
            self.a = a

    # ======================================== #
    # test methods
    # ======================================== #
    def test_reference_equal(self):
        o1 = EqualIsTest.ReferenceBasedEqualObj(9)
        self.assertEqual(9,o1.a)

        o2 = EqualIsTest.ReferenceBasedEqualObj(9)
        self.assertEqual(o2.a,o1.a)

        self.assertTrue(o1 != o2)
        self.assertFalse(o1 == o2)

        sameref = o1
        self.assertIs(sameref,o1)
        self.assertTrue(sameref == o1)# "==" is based on reference equal

    def test_is(self):
        """is: check on reference equality"""
        o = EqualIsTest.ReferenceBasedEqualObj(99)
        sameContent = EqualIsTest.ReferenceBasedEqualObj(99)
        sameRef = o
        
        self.assertTrue(o != sameContent)
        self.assertTrue(o == sameRef)
        self.assertTrue(o is sameRef)
        self.assertEqual(id(o),id(sameRef))

    def test_content_equal(self):
        o1 = EqualIsTest.ContentBasedEqualObj(9)
        o2 = EqualIsTest.ContentBasedEqualObj(9)

        self.assertTrue(o1 is not o2)
        self.assertEqual(o1,o2)
        self.assertTrue(o1 == o2)# __eq__ is overrided, then behaviour of "==" is changed

        o3 = EqualIsTest.ContentBasedEqualObj(8)
        self.assertTrue(o1 != o3)# because __ne__ is also overrided
        


    

