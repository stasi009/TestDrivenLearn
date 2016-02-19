package wsu.cheka.basictest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;

import org.junit.Test;

public class ReflectionTest {
	@Test
	public void testClassForName() throws ClassNotFoundException,
			IllegalAccessException, InstantiationException {
		String className = "wsu.cheka.basictest.MapTest";
		Class<? extends MapTest> cl = Class.forName(className).asSubclass(
				MapTest.class);

		assertEquals(className, cl.getName());
		assertFalse(cl.isInterface());
		assertEquals("wsu.cheka.basictest", cl.getPackage().getName());

		MapTest tester = cl.newInstance();
		assertTrue(cl.isInstance(tester));
	}

	@Test
	public void testInterfaceForName() throws ClassNotFoundException,
			IllegalAccessException, InstantiationException {
		String interfaceName = "java.util.List";
		Class cl = Class.forName(interfaceName);
		assertEquals(interfaceName, cl.getName());
		assertTrue(cl.isInterface());
	}

	@Test
	public void testDynamicConstruct() throws ClassNotFoundException,
			IllegalAccessException, InstantiationException,
			NoSuchMethodException, InvocationTargetException {
		// get class meta information
		Class<? extends StudentInfo> cl = Class.forName(
				"wsu.cheka.basictest.StudentInfo")
				.asSubclass(StudentInfo.class);

		// get constructor
		Class[] parmTypes = new Class[2];
		parmTypes[0] = String.class;
		parmTypes[1] = Integer.TYPE;
		Constructor cons = cl.getConstructor(parmTypes);

		// prepare parameters for constructor
		String srcName = "cheka";
		int srcID = 8;
		Object[] parmValues = new Object[] { srcName, Integer.valueOf(srcID) };

		// invoke the constructor to generate an object
		Object obj = cons.newInstance(parmValues);
		assertTrue(obj instanceof StudentInfo);

		StudentInfo studinfo = (StudentInfo) obj;
		assertEquals(srcName, studinfo.getName());
		assertEquals(srcID, studinfo.getID());
	}

	@Test
	public void testDynamicInvokeMethod() throws ClassNotFoundException,
			NoSuchMethodException, InvocationTargetException,
			IllegalAccessException {
		// original object
		String name = "cheka";
		int id = 88;
		StudentInfo studinfo = new StudentInfo(name, id);
		Class cl = StudentInfo.class;

		// get and invoke "getter"
		Method getNameMethod = cl.getMethod("getName", new Class[0]);
		String nameGot = (String) getNameMethod.invoke(studinfo, new Object[0]);
		assertEquals(name, nameGot);

		// get and invoke "setter"
		int newid = 99;
		Method setIdMethod = cl
				.getMethod("setID", new Class[] { Integer.TYPE });
		setIdMethod.invoke(studinfo, new Object[] { Integer.valueOf(newid) });
		assertEquals(newid, studinfo.getID());
	}

	@Test
	public void testAccessPrivateField() throws NoSuchFieldException,
			IllegalAccessException {
		StudentInfo studinfo = new StudentInfo("cheka", 88);
		int newid = 99;

		Class cl = StudentInfo.class;
		Field idField = cl.getDeclaredField("m_id");
		idField.setAccessible(true);
		idField.setInt(studinfo, newid);

		assertEquals(99, studinfo.getID());
	}
}
