name := "scala-demo-apps"

version := "1.0"

scalaVersion := "2.11.8"

scalacOptions ++= Seq("-unchecked", "-deprecation")

EclipseKeys.eclipseOutput := Some(".target")

libraryDependencies ++= {
  val akkaVersion = "2.5.1"
  Seq(
    "com.typesafe.akka" %% "akka-actor"      % akkaVersion,
    "com.typesafe.akka" %% "akka-agent"      % akkaVersion,
    "com.typesafe.akka" %% "akka-contrib"    % akkaVersion,
    "com.typesafe.akka" %% "akka-camel"      % akkaVersion,
    "com.typesafe.akka" %% "akka-remote"     % akkaVersion,
    "com.typesafe.akka" %% "akka-slf4j"      % akkaVersion,
    "com.typesafe.akka" %% "akka-testkit"    % akkaVersion   % "test"
  )
}
