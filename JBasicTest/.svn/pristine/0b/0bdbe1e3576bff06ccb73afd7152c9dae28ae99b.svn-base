package wsu.cheka.basictest;

import org.junit.Test;
import static org.junit.Assert.*;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.Random;

public final class CollectionTest {
	private static final class TestElement {
		private int m_id;

		public TestElement(int id) {
			m_id = id;
		}

		@Override
		public boolean equals(Object otherobj) {
			if (otherobj == null)
				return false;
			if (this == otherobj)
				return true;
			if (!(otherobj instanceof TestElement))
				return false;

			TestElement otherElement = (TestElement) otherobj;
			return m_id == otherElement.m_id;
		}

		@Override
		public int hashCode() {
			return m_id;
		}
	}
	
	private void checkSame(List<?> srcList,List<?> cpyList)
	{
		assertEquals(srcList.size(), cpyList.size());
		
		Iterator<?> srcIt = srcList.iterator();
		Iterator<?> cpyIt = cpyList.iterator();
		
		int index = 0;
		while (srcIt.hasNext() && cpyIt.hasNext())
		{
			++ index;
			assertSame(srcIt.next(), cpyIt.next());
		}
		assertEquals(index, srcList.size());
	}

	@Test
	public void testAlwaysShallowCopy() {
		Random random = new Random();
		List<TestElement> srcList = new ArrayList<TestElement>();
		srcList.add(new TestElement(random.nextInt()));
		srcList.add(new TestElement(random.nextInt()));

		// ***************** shallow copy constructor
		List<TestElement> listCopyByConstructor = new LinkedList<TestElement>(srcList);
		checkSame(srcList, listCopyByConstructor);

		// ***************** also shallow copy
		List<TestElement> listCopyByMethod = new LinkedList<TestElement>();
		for (int index = 0; index < srcList.size(); ++index)
			listCopyByMethod.add(null);
		
		// !!!!!!!! check the source codes of "Collections.copy", we can find that
		// !!!!!!!! this method is also doing a shallow copy, and seems no optimization
		Collections.copy(listCopyByMethod, srcList);
		checkSame(srcList, listCopyByMethod);
	}
}
