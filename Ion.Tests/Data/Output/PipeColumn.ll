; ModuleID = 'entry'
source_filename = "entry"

@.str.0 = private unnamed_addr constant [12 x i8] c"Test string\00"

define void @Test1(i8* %input) {
entry:
  ret void
}

define void @main() {
entry:
  %.anonymous.7 = call void @Test1(i8* getelementptr inbounds ([12 x i8], [12 x i8]* @.str.0, i32 0, i32 0))
  ret void
}
