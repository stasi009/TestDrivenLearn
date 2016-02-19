package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotSame;
import static org.junit.Assert.assertSame;
import static org.junit.Assert.assertTrue;

import java.util.Arrays;
import java.util.Random;

import org.junit.Test;

public class CloneTest {
	/**
	 * Cloneable has no method defined clone method is declared in Object, not
	 * in Cloneable interface Cloneable interface is just a indicator, a marker
	 * if an object has not implemented Cloneable interface then call Clone will
	 * throw out CloneNotSupportedException exception
	 */
	static class Element implements Cloneable {
		private int m_number;

		/**
		 * clone is not implemented with Constructor
		 * so there is no limitation to a class which implements Cloneable
		 * even this class has no default constructor is still OK
		 * @param number
		 */
		public Element(int number) {
			m_number = number;
		}

		public void setNumber(int number) {
			m_number = number;
		}

		public int getNumber() {
			return m_number;
		}

		public boolean equals(Object other) {
			if (other == this)
				return true;

			if (!(other instanceof Element))
				return false;

			final Element elementObj = (Element) other;
			return m_number == elementObj.m_number;
		}

		public int hashCode() {
			return m_number;
		}

		/**
		 * CloneNotSupportedException is derived directly from Exception, not
		 * RuntimeException, so is a checked exception must be handled
		 * explicitly
		 */
		public Element clone() throws CloneNotSupportedException {
			return (Element) super.clone();
		}
	}

	/**
	 * define the common member variables and member methods shared by both
	 * shallow-copy container and deep-copy container
	 * 
	 * @author cheka
	 * 
	 */
	static class BaseContainer implements Cloneable {
		protected Element m_element;
		protected int m_id;
		protected int[] m_array = new int[10];

		public BaseContainer(int id, int number) {
			m_id = id;
			m_element = new Element(number);

			Random rand = new Random(System.currentTimeMillis());
			for (int index = 0; index < m_array.length; ++index) {
				m_array[index] = rand.nextInt();
			}
		}

		public int getId() {
			return m_id;
		}

		public Element getElement() {
			return m_element;
		}

		public int[] getArray() {
			return m_array;
		}

		@Override
		public boolean equals(Object other) {
			if (other == null)
				return false;

			if (this == other)
				return true;

			if (this.getClass() != other.getClass())
				return false;

			final BaseContainer otherContainer = (BaseContainer) other;
			return (m_id == otherContainer.m_id)
					&& (m_element.equals(otherContainer.m_element))
					&& (Arrays.equals(m_array, otherContainer.m_array));
		}

		@Override
		public int hashCode() {
			String idstring = Integer.toString(m_id)
					+ Integer.toString(m_element.hashCode())
					+ Arrays.toString(m_array);
			return idstring.hashCode();
		}

		protected BaseContainer clone() throws CloneNotSupportedException {
			return (BaseContainer) super.clone();
		}
	}

	/**
	 * we must implement Cloneable, otherwise call object's clone willl throw
	 * CloneNotSupportedException
	 */
	static class ShallowCopyContainer extends BaseContainer implements
			Cloneable {
		public ShallowCopyContainer(int id, int number) {
			super(id, number);
		}

		/**
		 * object's clone is not just copy content but can make sure that the
		 * "Object" returned by 'clone' guarantee that
		 * "obj.clone().getClass() == obj.getClass()"
		 * 
		 * and this method use the new override feature in Java which is that
		 * the override method can have different return type, as long as the
		 * new return type can assign to the base version return type
		 */
		public ShallowCopyContainer clone() throws CloneNotSupportedException {
			Object obj = super.clone();
			assertTrue(obj instanceof ShallowCopyContainer);
			return (ShallowCopyContainer) obj;
		}
	}

	static class DeepCopyContainer extends BaseContainer implements Cloneable {
		public DeepCopyContainer(int id, int number) {
			super(id, number);
		}

		@Override
		public DeepCopyContainer clone() throws CloneNotSupportedException {
			Object obj = super.clone();
			assertTrue(obj instanceof DeepCopyContainer);
			DeepCopyContainer cpyContainer = (DeepCopyContainer) obj;

			if (m_element != null) {
				cpyContainer.m_element = m_element.clone();
			}

			if (m_array != null) {
				cpyContainer.m_array = Arrays.copyOf(m_array, m_array.length);
			}
			return cpyContainer;
		}
	}

	@Test
	public void testShallowClone() throws CloneNotSupportedException {
		// "=" cannot be overriden in Java
		// implement "Cloneable" cannot change the fact
		// "=" do the reference assignment, not value copy or clone
		// no new object is created
		BaseContainer srcContainer = new ShallowCopyContainer(1, 2);
		BaseContainer referenceCpy = srcContainer;
		assertSame(referenceCpy, srcContainer);

		BaseContainer cpyContainer = srcContainer.clone();
		assertTrue(cpyContainer instanceof ShallowCopyContainer);
		assertNotSame(srcContainer, cpyContainer);
		assertEquals(srcContainer.getId(), cpyContainer.getId());

		// if the member variable is reference type
		// then object's clone just do shallow copy on this member variable
		// then this reference member variable will be shared both
		// by source object and copy object
		assertSame(srcContainer.getElement(), cpyContainer.getElement());
		assertTrue(srcContainer.getElement() == cpyContainer.getElement());
		assertTrue(srcContainer.getElement().equals(cpyContainer.getElement()));

		// array belongs to reference type, so still shallow copy
		assertSame(srcContainer.getArray(), cpyContainer.getArray());

		assertTrue(srcContainer.equals(cpyContainer));
	}

	/**
	 * clone is totally dynamic binding although called via base class reference
	 * but the object returned by "clone" is absolutely child object
	 */
	@Test
	public void testDeepClone() throws CloneNotSupportedException {
		BaseContainer srcContainer = new DeepCopyContainer(100, 200);
		BaseContainer cpyContainer = srcContainer.clone();

		// clone can keep the class's meta information
		assertTrue(cpyContainer instanceof DeepCopyContainer);

		assertNotSame(srcContainer, cpyContainer);
		assertTrue(srcContainer.equals(cpyContainer));
		assertEquals(srcContainer, cpyContainer);

		assertNotSame(srcContainer.m_element, cpyContainer.m_element);
		assertNotSame(srcContainer.m_array, cpyContainer.m_array);
	}
}
