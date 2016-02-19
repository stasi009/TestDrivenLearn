package wsu.cheka.basictest;

import java.util.Scanner;

/**
 * first java code for basic test test the syntax and usage of console IO in
 * java
 * 
 * @author cheka
 * @date 2010-3-12,Friday
 */
public class StandardIOApp {
	/**
	 * test the syntax for outputing to console
	 */
	private static void testOutput() {
		System.out.println("Hello Java From WSU");
		System.out.printf("Hello Java From %s\n", "Cheka");
		System.out.printf("Hello %d times From %s\n", 10, "Cheka");

		System.err
				.println("!!! Information sent to standard error will never be re-directed to other devices");
	}

	/**
	 * test the syntax for reading from console
	 */
	private static void testInput() {
		Scanner reader = new Scanner(System.in);

		System.out.println("Please input your name:");
		System.out.printf("*** Hello %s\n", reader.next());

		System.out.println("Please input your ID:");
		System.out.printf("*** Your ID is %d\n", reader.nextInt());

		System.out.println("Please input your score:");
		System.out.printf("*** Your score is %f\n", reader.nextFloat());

		System.out.println("Please input whether you agree or not:");
		System.out.println(reader.nextBoolean());
	}

	public static void main(String[] args) {
		testOutput();
		testInput();
		System.out.println("========= program exits =========");
	}
}
