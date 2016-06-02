
import unittest
import math

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

    def test_float_numbers(self):
        # using position arguments
        self.assertEqual('pi = 3.14, e = 2.718',"pi = {0:.2f}, e = {1:.3f}".format(math.pi,math.e))

        # using named arguments
        self.assertEqual('pi = 3.142, e = 2.72',"pi = {pi:.3f}, e = {e:.2f}".format(pi = math.pi,e = math.e))

    def test_width_control(self):
        # left alignment
        self.assertEqual('a     is first',"{:5} is first".format("a"))

        # right alignment
        self.assertEqual('    b is second',"{:>5} is second".format("b"))

        # specify fill characters
        self.assertEqual('a**** is first',"{:*<5} is first".format("a"))
        self.assertEqual('****b is second',"{:*>5} is second".format("b"))

        # width can be also specified dynamically
        self.assertEqual('a     is first',"{0:{1}} is first".format("a",5))
        self.assertEqual('a     is first',"{content:{width}} is first".format(width=5,content="a"))




if __name__ == "__main__":
    unittest.main()


