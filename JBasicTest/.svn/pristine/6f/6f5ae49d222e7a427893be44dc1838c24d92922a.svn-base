package wsu.cheka.basictest;

import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertNotSame;
import static org.junit.Assert.assertNull;
import static org.junit.Assert.assertTrue;

import java.util.Arrays;
import java.util.Comparator;
import java.util.List;
import java.util.Random;

import org.junit.Test;

public class ArrayTest {
	@Test
	public void testSingleDimArray() {
		int[] intArray = { 1, 2, 3, 4, 5 };
		assertEquals(5, intArray.length);
		assertEquals(2, intArray[1]);

		// test default value
		intArray = new int[10];
		for (int i = 0; i < intArray.length; ++i) {
			assertEquals(0, intArray[i]);
		}
	}

	@Test
	public void testRectangleArray() {
		int[][] recArray = { { 1, 2, 3 }, { 4, 5, 6 } };

		assertEquals(2, recArray.length);
		assertEquals(3, recArray[0].length);
		assertEquals(recArray[0].length, recArray[1].length);

		int count = 0;
		for (int row = 0; row < recArray.length; ++row) {
			for (int col = 0; col < recArray[0].length; ++col) {
				++count;
				assertEquals(count, recArray[row][col]);
			}
		}
	}

	@Test
	public void testJaggedArray() {
		int numRow = 4;
		int numCol0 = 5;
		int numCol1 = 6;

		int[][] jaggedArray = new int[numRow][];
		assertEquals(numRow, jaggedArray.length);
		for (int row = 0; row < jaggedArray.length; row++) {
			assertNull(jaggedArray[row]);
		}

		jaggedArray[0] = new int[numCol0];
		assertEquals(numCol0, jaggedArray[0].length);

		jaggedArray[1] = new int[numCol1];
		assertEquals(numCol1, jaggedArray[1].length);
	}

	@Test
	public void testArrayCopy() {
		int[] originalArray = { 1, 2, 3, 4, 5 };
		int[] copiedArray = new int[5];
		System.arraycopy(originalArray, 0, copiedArray, 0,originalArray.length);

		assertNotSame(originalArray, copiedArray);
		// only array needs special "assertArrayEquals"
		// because List has already overriden "equals"
		// so List can use "assertEquals" directly
		assertArrayEquals(copiedArray, originalArray);
	}

	private int[] createRandomArray(int size) {
		Random randGenerator = new Random(System.currentTimeMillis());

		int[] intArray = new int[size];
		for (int index = 0; index < intArray.length; ++index) {
			intArray[index] = randGenerator.nextInt();
		}

		return intArray;
	}

	@Test
	public void testArrayCopyOf() {
		int[] srcArray = createRandomArray(25);
		int[] copyArray = Arrays.copyOf(srcArray, srcArray.length);

		// assert when the copied array has the same length
		// with the original source array
		assertNotSame(copyArray, srcArray);
		// array's equals still check the reference equality
		assertFalse(srcArray.equals(copyArray));
		assertTrue(Arrays.equals(copyArray, srcArray));
		assertArrayEquals(copyArray, srcArray);

		// assert when the copied array has longer size
		// than the original source array
		copyArray = Arrays.copyOf(srcArray, srcArray.length * 2);
		int index = 0;
		for (index = 0; index < srcArray.length; ++index) {
			assertEquals(srcArray[index], copyArray[index]);
			// for the range exceeding the original length
			// just fill the default value
			assertEquals(0, copyArray[srcArray.length + index]);
		}
	}

	@Test
	public void testUserDefinedSort() {
		String[] nameArray = new String[] { "stasi", "cheka", "nkvd", "kgb" };
		String[] ascendArray = new String[] { "cheka", "kgb", "nkvd", "stasi" };

		// ascend sorting
		Arrays.sort(nameArray);
		int index = 0;
		for (String name : nameArray) {
			assertEquals(ascendArray[index], name);
			++index;
		}

		// descend sorting
		Comparator<String> descendSorter = new Comparator<String>() {
			public int compare(String s1, String s2) {
				return s2.compareTo(s1);
			}
		};
		Arrays.sort(nameArray, descendSorter);
		index = nameArray.length - 1;
		for (String name : nameArray) {
			assertEquals(ascendArray[index], name);
			--index;
		}
	}

	@Test
	public void testSortAndSearch() {
		int[] intArray = createRandomArray(20);
		Arrays.sort(intArray);

		int index = 0;
		for (index = 0; index < intArray.length - 1; ++index) {
			assertTrue(intArray[index] <= intArray[index + 1]);
		}

		int findIndex = 0;
		for (index = 0; index < intArray.length; ++index) {
			findIndex = Arrays.binarySearch(intArray, intArray[index]);
			assertEquals(index, findIndex);
		}
	}

	@Test
	public void testFill() {
		int[] intArray = new int[10];
		int number = 78;
		Arrays.fill(intArray, number);

		for (int index = 0; index < intArray.length; ++index) {
			assertEquals(number, intArray[index]);
		}
	}

	@Test
	public void testEquals() {
		int number = 88;
		int size = 15;
		int[] array1 = new int[size];
		Arrays.fill(array1, number);

		int[] array2 = new int[size];
		Arrays.fill(array2, number);

		assertNotSame(array1, array2);
		assertFalse(array1 == array2);

		// !!!!!!!!!!! PAY ATTENTION THAT
		// !!!!!!!!!!! ARRAY has not override "equals"
		// !!!!!!!!!!! it's the same in C#
		// !!!!!!!!!!! in C#,System.Array doesn't override
		// !!!!!!!!!!! object's Equals either
		assertFalse(array1.equals(array2));

		assertTrue(Arrays.equals(array1, array2));
		assertArrayEquals(array1, array2);
	}

	@Test(expected = ArrayIndexOutOfBoundsException.class)
	public void testIndexException() {
		int[] array = new int[0];
		int value = array[0];
	}

	@Test
	public void testDeepEquals() {
		int[][] arr1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
		int[][] arr2 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
		int[][] arr3 = { { 0, 1, 3 }, { 4, 6, 4 }, { 7, 8, 9 } };

		assertTrue(Arrays.deepEquals(arr1, arr2));
		assertFalse(Arrays.deepEquals(arr1, arr3));
	}

	@Test
	public void testDefaultValue() {
		int[] intArray = new int[10];
		for (int intval : intArray) {
			assertEquals(0, intval);
		}

		Object[] objArray = new Object[20];
		for (Object objValue : objArray) {
			assertNull(objValue);
			assertEquals(null, objValue);
		}
	}

	@Test
	public void testForeach() {
		int[] intArray = createRandomArray(30);
		int index = 0;
		for (int number : intArray) {
			assertEquals(intArray[index], number);
			++index;
		}
	}

	/**
	 * unlike C#, in java, array and collection don't share the same interface
	 * so array cannot be used directly in the occasion that needs a iterable, or collection
	 * this is why asList is useful, this method just return a internal list which can be used
	 * when collection is needed.
	 */
	@Test
	public void testAsList()
	{
		// test object array
		String[] strArray = new String[] {"cheka","kgb","stasi","mss"};
		List<String> strList = Arrays.asList(strArray);
		assertEquals(strArray.length, strList.size());
		
		// test array of primitive types
		final int count = 10;
		int[] primIntArray = new int[count];
		List primIntList = Arrays.asList(primIntArray);
		List<int[]> intarrList = Arrays.asList(primIntArray);
		// !!! intList is not List<Integer>, but List<int[]>, so the length is one
		// !!! this is because that asList take object as its argument
		// !!! but int is not object, but int[] is object, so when assigned to aslist
		// !!! the qualifed argument is only one
		assertEquals(1, primIntList.size());
		
		// test array of wrapper class for primitive type
		Integer[] integerArray = new Integer[count];
		List<Integer> integerList = Arrays.asList(integerArray);
		assertEquals(integerArray.length, integerList.size());
		assertEquals(count, integerList.size());
	}
}
