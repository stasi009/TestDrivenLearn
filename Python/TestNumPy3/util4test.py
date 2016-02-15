
import numpy as np

def compare_equal(expectedlist,actual_ndarray):
    expected_ndarray = np.array(expectedlist)
    return np.array_equal(expected_ndarray,actual_ndarray)

############# should use "np.allclose" instead
def compare_almost_equal(expectedlist,actual_ndarray,delta = 1e-6):
    expected_ndarray = np.array(expectedlist)
    compare = (np.abs(expected_ndarray - actual_ndarray)) <= delta
    return compare.all()

def assert_ndarray_equal(testcase,expectedlist,actual_ndarray):
    expected_ndarray = np.array(expectedlist)
    testcase.assertTrue( np.array_equal(expected_ndarray,actual_ndarray) )

def assert_ndarray_almost_equal(testcase,expectedlist,actual_ndarray,delta = 1e-6):
    expected_ndarray = np.array(expectedlist)
    compare = (np.abs(expected_ndarray - actual_ndarray)) <= delta
    testcase.assertTrue( compare.all() )