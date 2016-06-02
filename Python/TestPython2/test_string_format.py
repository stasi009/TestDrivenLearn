
import unittest
import math
from datetime import datetime

class StringFormatTest(unittest.TestCase):

    def test_position_parameter(self):
        s = "my name is {0}, I am {1}".format("stasi","Chinese")
        self.assertEqual('my name is stasi, I am Chinese',s)

        # to include a literal { or } in the string, you double it to {{ or }}
        # below {{{0}}}, the outside {{ and }} represents literal {}
        # and the inner most {0} represents the string which is going to be
        # replaced
        s = "my name is {{{0}}}, I am {{{1}}}".format("stasi","Chinese")
        self.assertEqual('my name is {stasi}, I am {Chinese}',s)

        # number can be ignored if arguments are in order
        self.assertEqual('a, b, c','{}, {}, {}'.format('a', 'b', 'c'))

        # reverse the order
        self.assertEqual('c, b, a', '{2}, {1}, {0}'.format(*'abc'))

        # arguments' indices can be repeated
        self.assertEqual('xyx', '{0}{1}{0}'.format('x', 'y'))   


    def test_named_parameter(self):
        s = "my name is {name}, I am {nation}".format(name="stasi",nation ="Chinese")
        self.assertEqual('my name is stasi, I am Chinese',s)

        # arguments can be passed in as dictionary
        coord = {'latitude': 37.2408, 'longitude': -115.8197}
        s = 'Coordinates: ({latitude:.2f}N, {longitude:.2f}W)'.format(**coord)
        self.assertEqual("Coordinates: (37.24N, -115.82W)",s)

        # mix position argument and named argument
        # and can access attributes of the argument
        s = "{0} is the food of {users[1]}".format("Ambrosia",users=["men", "the gods", "others"])
        self.assertEqual("Ambrosia is the food of the gods",s)

    def test_access_attributes(self):
        person = {'first': 'Jean-Luc', 'last': 'Picard'}
        self.assertEqual("Jean-Luc Picard"  ,'{p[first]} {p[last]}'.format(p=person))

        data = [4, 8, 15, 16, 23, 42]
        self.assertEqual("23 42",'{d[4]} {d[5]}'.format(d=data))

    def test_datetime(self):
        s = '{:%Y-%m-%d %H:%M}'.format(datetime(2001, 2, 3, 4, 5))
        self.assertEqual("2001-02-03 04:05",s)

    def test_integer(self):
        self.assertEqual('count=998',"count={}".format(998))
        # seems no difference after adding ":d"
        self.assertEqual('count=699',"count={:d}".format(699))
        self.assertEqual('x=    99    ', "x={:^10}".format(99))

        # padding with zero
        self.assertEqual('0089', '{:04d}'.format(89))

        # show plus sign
        self.assertEqual('+9', '{:+d}'.format(9))

    def test_float(self):
        # basic, no precision control
        self.assertEqual('value=1.234', "value={}".format(1.234))

        # using position arguments
        # 'f' is necessary, otherwise it controls the whole width, not
        # precision, and will truncate
        self.assertEqual('pi=3.14, e=2.718',"pi={0:.2f}, e={1:.3f}".format(math.pi,math.e))

        # using named arguments
        self.assertEqual('pi=3.142, e=2.72',"pi={pi:.3f}, e={e:.2f}".format(pi = math.pi,e = math.e))

        # control both width and precision
        self.assertEqual('pi= 3.142  , e=  2.72  ',"pi={pi:^8.3f}, e={e:^8.2f}".format(pi = math.pi,e = math.e))

        # pad with zero
        self.assertEqual('pi=03.14200, e=002.7200',"pi={pi:^08.3f}, e={e:^08.2f}".format(pi = math.pi,e = math.e))

    def test_truncate_string(self):
        # number behind '.', for float, it controls precision, for string, it
        # controls the length
        self.assertEqual("12", "{:.2}".format("123456789"))

        # width can also be pass in dynamically
        self.assertEqual("123", "{:.{}}".format("123456789",3))

        # combine truncate and padding together
        # number before .  controls the whole width, number behind .  controls
        # truncating
        self.assertEqual(" 123 ", "{:^5.3}".format("123456789"))

    def test_width_control(self):
        # left alignment
        self.assertEqual('a     is first',"{:5} is first".format("a"))

        # right alignment
        self.assertEqual('    b is second',"{:>5} is second".format("b"))

        # center alignment
        self.assertEqual('  c   is third',"{:^5} is third".format("c"))

        # specify fill characters
        self.assertEqual('a**** is first',"{:*<5} is first".format("a"))
        self.assertEqual('****b is second',"{:*>5} is second".format("b"))

        # width can be also specified dynamically
        self.assertEqual('a     is first',"{0:{1}} is first".format("a",5))
        self.assertEqual('a     is first',"{content:{width}} is first".format(width=5,content="a"))

    def test_object_str_repr(self):
        class Data(object):
            def __init__(self,v): self.v = v
            def __str__(self): return 'str{}'.format(self.v)
            def __repr__(self): return 'repr{}'.format(self.v)

        self.assertEqual('str1 repr2', '{0!s} {1!r}'.format(Data(1),Data(2)))

    def test_custom_format(self):
        """
        You can define custom format handling in your own objects by overriding __format__()
        """
        class Data(object):
            def __init__(self,v) : self.v = v
            def __format__(self, format):                 return "{}{}".format(format,self.v)

        d = Data(99)
        self.assertEqual('default prefix: 99, specified prefix: zz99', "default prefix: {0}, specified prefix: {0:zz}".format(d))


if __name__ == "__main__":
    unittest.main()


