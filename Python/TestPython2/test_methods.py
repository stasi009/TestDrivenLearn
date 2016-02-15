
import unittest
import packages1.circle as circle

class MethodsTest(unittest.TestCase):

    def test_demo_static_method(self):
        c1 = circle.Circle()
        c2 = circle.Circle(2)
        self.assertEqual(2,circle.Circle.totalCircles())
        self.assertAlmostEqual(15.70795,circle.Circle.totalArea())
        
        c2.radius = 3
        self.assertAlmostEqual(31.415899999999997,circle.Circle.totalArea())

    def test_instance_method(self):
        class Fool(object):
            def __init__(self,size):
                self.size = size
            def get_size(self): 
                return self.size
        p1 = Fool(9)
        # --------------- normal way
        self.assertEqual(9,p1.get_size())

        # --------------- second way
        self.assertEqual(9,Fool.get_size(p1))

        # --------------- functional way
        f = p1.get_size
        self.assertEqual(9,f())

        # --------------- instance methods are bound to different object
        # --------------- they are different object, will occupy different
        # memory space
        p2 = Fool(9)
        self.assertTrue(p1.get_size is not p2.get_size)

    def test_static_method(self):
        class Smoothie(object):

            YOGURT = 1
            STRAWBERRY = 2
            BANANA = 4
            MANGO = 8

            @staticmethod
            def blend(*mixes):
                return sum(mixes) / len(mixes)

            @staticmethod
            def eternal_sunshine():
                return Smoothie.blend(Smoothie.YOGURT, Smoothie.STRAWBERRY, Smoothie.BANANA)

            @staticmethod
            def mango_lassi():
                return Smoothie.blend(Smoothie.YOGURT, Smoothie.MANGO)

        self.assertEqual(2,Smoothie.eternal_sunshine())
        self.assertEqual(4,Smoothie.mango_lassi())

    def test_class_method(self):
        """
        use classmethod as staticmethod, but in a better way
        since we can reference all static member fields and static member methods by using "cls"
        other than the whole class name, so when we need to "rename" the class
        classmethods don't need to be changed, while staticmethods have to be changed to use the new name
        """
        class Smoothie(object):
            YOGURT = 1
            STRAWBERRY = 2
            BANANA = 4
            MANGO = 8

            @staticmethod
            def blend(*mixes):
                return sum(mixes) / len(mixes)

            @classmethod
            def eternal_sunshine(cls):
                return cls.blend(cls.YOGURT, cls.STRAWBERRY, cls.BANANA)

            @classmethod
            def mango_lassi(cls):
                return cls.blend(cls.YOGURT, cls.MANGO)

        self.assertEqual(2,Smoothie.eternal_sunshine())
        self.assertEqual(4,Smoothie.mango_lassi())


