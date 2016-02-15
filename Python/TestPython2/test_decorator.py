
import unittest

class Resistor(object):
    def __init__(self,ohms):
        self.ohms = ohms # property-setter invoked

    @property
    def ohms(self):
        return self._ohms

    @ohms.setter
    def ohms(self,value):
        if value <= 0 :
            raise ValueError("ohms=%3.2f, <=0" % value)
        self._ohms = float(value)

    @property
    def voltage(self):
        return self._voltage

    @voltage.setter
    def voltage(self,value):
        self._voltage = float(value)
        self.current = self._voltage / self._ohms

class DecoratorTest(unittest.TestCase):

    def test_property(self):
        with self.assertRaises(ValueError):
            _ = Resistor(-1)

        o = Resistor(5)
        o.voltage = 10
        self.assertAlmostEqual(5.0,o.ohms)
        self.assertAlmostEqual(10.0,o.voltage)
        self.assertAlmostEqual(2.0,o.current)



