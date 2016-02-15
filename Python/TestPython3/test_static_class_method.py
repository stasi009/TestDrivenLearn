
import unittest
import packages1.circle as circle

########################################################################
class StaticClassMethodTest(unittest.TestCase):
    """demonstrate the usage of static method and class method"""
    
    def testStaticMethod(self):
        c1 = circle.Circle()
        c2 = circle.Circle(2)
        self.assertEqual(2,circle.Circle.totalCircles())
        self.assertAlmostEqual(15.70795,circle.Circle.totalArea())
        
        c2.radius = 3
        self.assertAlmostEqual(31.415899999999997,circle.Circle.totalArea())
    
    

        
        
    
    