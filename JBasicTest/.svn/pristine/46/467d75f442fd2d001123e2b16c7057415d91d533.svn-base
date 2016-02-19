package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;

import java.util.Random;

import org.junit.Test;

public class GenericTest {
	private static interface IValueObject<T> {
		T getValue();

		void setValue(T value);
	}

	/**
	 * <T> equals to <T extends Object> so only object can be used in generic
	 * primitives is forbidden.
	 */
	private static class GenericValueObject<T> implements IValueObject<T> {
		private T m_value;

		public void setValue(T value) {
			m_value = value;
		}

		public T getValue() {
			return m_value;
		}
	}

	private static class StringValueObject implements IValueObject<String> {
		private String m_value;

		public void setValue(String value) {
			m_value = value;
		}

		public String getValue() {
			return m_value;
		}
	}

	/**
	 * pay attention that Generic in Java doesn't support primitive type only
	 * the class type can be the generic type so int, float cannot be put into a
	 * generic container, only Integer, Float can be stored in a generic
	 * container
	 * 
	 * but the good news is that this limitation only takes its effect when
	 * declaring a generic object, when use it, we can still use primitive type,
	 * because auto-boxing and auto-unboxing will happen
	 */
	@Test
	public void testGenericOnPrimitiveTypes() {
		int number = 100;
		GenericValueObject<Integer> intValObj = new GenericValueObject<Integer>();

		intValObj.setValue(number);// auto-boxing happen
		int getval = intValObj.getValue();// auto-unboxing happen

		assertEquals(number, getval);
	}

	/**
	 * because that T cannot be primitive type, so the clause like "a > b"
	 * cannot pass the compilation, we must designate the interface T must
	 * implements and call specific method defined in that interface
	 */
	private static <T extends Comparable<T>> T Max(T a, T b) {
		return a.compareTo(b) > 0 ? a : b;
	}

	@Test
	public void testGenericMethod() {
		Random rand = new Random(System.currentTimeMillis());

		int nval1 = rand.nextInt();
		int nval2 = rand.nextInt();
		assertEquals(nval1 > nval2 ? nval1 : nval2, (int) Max(nval1, nval2));

		float fval1 = rand.nextFloat();
		float fval2 = rand.nextFloat();
		assertEquals(fval1 > fval2 ? fval1 : fval2, (float) Max(fval1, fval2),
				1e-6);
	}

	@Test
	public void testGenericCast() {
		GenericValueObject<Integer> intObj = new GenericValueObject<Integer>();
		GenericValueObject<Float> floatObj = new GenericValueObject<Float>();

		GenericValueObject genericRef = intObj;
		genericRef = floatObj;

		GenericValueObject<? extends Number> numberRef = intObj;
		// "GenericValueObject<?>" is equivalent to "GenericValueObject"
		GenericValueObject<?> objRef = floatObj;
	}

	@Test
	public void testSpecificValueObj() {
		StringValueObject strobj = new StringValueObject();
		IValueObject genericref = strobj;

		// method can be invoked via generic interface
		// no need to bind to one specific type instance
		String name = "cheka";
		strobj.setValue("KGB");
		genericref.setValue(name);

		assertEquals(name, genericref.getValue());
	}
}
