package wsu.cheka.basictest;

import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;
import static org.junit.Assert.assertNotSame;
import static org.junit.Assert.assertSame;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;

import org.junit.Test;

public class ListTest {
	private static abstract class ListGeneratorBase {
		protected abstract List<String> createList();

		public List<String> makeList() {
			List<String> strList = createList();

			strList.add("cheka");
			strList.add("KGB");
			strList.add("STASI");
			strList.add("NKVD");

			return strList;
		}
	}

	private static interface IIterateChecker {
		void check(List<String> strList);
	}

	static class ArrayListGenerator extends ListGeneratorBase {
		@Override
		protected List<String> createList() {
			return new ArrayList<String>();
		}
	}

	static class LinkedListGenerator extends ListGeneratorBase {
		@Override
		protected List<String> createList() {
			return new LinkedList<String>();
		}
	}

	static class IteratorChecker implements IIterateChecker {
		public void check(List<String> strList) {
			int index = 0;
			Iterator<String> it = strList.iterator();
			while (it.hasNext()) {
				assertTrue(strList.get(index).equals(it.next()));
				++index;
			}
		}
	}

	/**
	 * find another difference between C# and java
	 * in C#, List can use [] to access the element
	 * but in java, only array can use [], but List cannot use []
	 * can only use "get" and "set"
	 * @author cheka
	 */
	static class ForeachChecker implements IIterateChecker {
		public void check(List<String> strList) {
			int index = 0;
			for (String name : strList) {
				assertEquals(name, strList.get(index));
				++index;
			}
		}
	}

	private void testList(ListGeneratorBase generator, IIterateChecker checker) {
		checker.check(generator.makeList());
	}

	@Test
	public void testArrayListIterator() {
		testList(new ArrayListGenerator(), new IteratorChecker());
	}

	@Test
	public void testLinkedListIterator() {
		testList(new LinkedListGenerator(), new IteratorChecker());
	}

	@Test
	public void testArrayListForeach() {
		testList(new ArrayListGenerator(), new ForeachChecker());
	}

	@Test
	public void testLinkedListForeach() {
		testList(new LinkedListGenerator(), new ForeachChecker());
	}

	/**
	 * list has override equals, to perform logic equality checking
	 * not just comparing reference address
	 * two list are equal as long as they are both list
	 * and they have the same number of elements, and the element at each position are equal
	 */
	@Test
	public void testEquals()
	{
		List<String> srclist = new ArrayList<String>();
		srclist.add("cheka");
		srclist.add("kgb");
		srclist.add("stasi");
		
		// to ensure that elements in both list will not reference to the same object
		// then this test can prove that two list check equality based on content comparison
		List<String> cpylist = new LinkedList<String>();
		for (String item : srclist)
			cpylist.add(new String(item));
		
		for (int index = 0; index < srclist.size(); ++index)
		{
			assertNotSame(srclist.get(index), cpylist.get(index));
		}
		
		assertNotSame(srclist, cpylist);
		assertEquals(srclist, cpylist);
	}
	
	@Test
	public void testShallowCopyConstructor()
	{
		List<String> srclist = new ArrayList<String>();
		srclist.add("cheka");
		srclist.add("kgb");
		
		List<String> cpyList = new LinkedList<String>(srclist);
		
		assertEquals(srclist.size(), cpyList.size());
		Iterator<String> srcIt = srclist.iterator();
		Iterator<String> cpyIt = cpyList.iterator();
		
		int index = 0;
		while (srcIt.hasNext() && cpyIt.hasNext())
		{
			String srcString = srcIt.next();
			String cpyString = cpyIt.next();
			assertSame(srcString,cpyString);
			++ index;
		}
		assertEquals(srclist.size(), index);
	}
	
	/**
	 * test the toArray method, which the result will be
	 * stored in the output parameter
	 */
	@Test
	public void testToArrayOutputParm() {
		String[] srcArray = { "cheka", "kgb", "stasi" };
		List<String> strlist = new ArrayList<String>();
		for (String name : srcArray) {
			strlist.add(name);
		}

		String[] cpyArray = new String[strlist.size()];
		strlist.toArray(cpyArray);

		assertArrayEquals(srcArray, cpyArray);
	}
	
	private List<StudentInfo> makeStudentInfoList()
	{
		List<StudentInfo> studlist = new ArrayList<StudentInfo>();
		studlist.add(new StudentInfo("cheka",1));
		studlist.add(new StudentInfo("henry",2));
		return studlist;
	}
	
	/**
	 * test the "toArray" method, which the result will be achieved
	 * by the return value
	 */
	@Test
	public void testToArrayShallowCopy()
	{
		List<StudentInfo> srcStudList = makeStudentInfoList();
		
		// input array is to small
		// so a new array is created, filled and returned
		StudentInfo[] cpyStudArray = srcStudList.toArray(new StudentInfo[0]);
		assertEquals(srcStudList.size(), cpyStudArray.length);
		
		for (int index = 0; index < cpyStudArray.length; ++index)
		{
			assertEquals(srcStudList.get(index), cpyStudArray[index]);
			assertSame(srcStudList.get(index), cpyStudArray[index]);
		}
	}
	
	@Test
	public void testSubList()
	{
		List<String> namelist = new LinkedList<String>();
		namelist.add("kgb");
		namelist.add("cheka");
		namelist.add("stasi");
		namelist.add("mss");
		
		List<String> expectedList = new ArrayList<String>();
		expectedList.add("kgb");
		expectedList.add("mss");
		
		// subList returns just a view
		// any change on the view will affect the underlying list
		namelist.subList(1, 3).clear();
		assertEquals(expectedList, namelist);
	}
}
