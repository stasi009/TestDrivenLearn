
import numpy as np
import unittest

class RandomTest(unittest.TestCase):
    
    def test_generated_shape(self):
        # pass any numbers, no need to wrap into a tuple
        a = np.random.rand(3,2)
        self.assertEqual((3,2),a.shape)
        
        # has to pass in a tuple
        b = np.random.random_sample((4,5))
        self.assertEqual((4,5),b.shape)
        
        # a single paramter, means single dimension
        c = np.random.random_sample(3)
        self.assertEqual((3,),c.shape)