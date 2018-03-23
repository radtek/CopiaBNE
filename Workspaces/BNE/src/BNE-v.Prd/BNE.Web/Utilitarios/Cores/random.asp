<%

Function fRandom()

dim myRnd(8)
makeRnd = 0
total = 0
valueExists = false

do while total < 8
	Randomize 
	makeRnd = Int((8 * rnd) + 1)
	for i = 0 to 7
		if cInt(makeRnd) = cInt(myRnd(i)) then
			valueExists = true
		end if
	next
	if valueExists = false then
		myRnd(total) = makeRnd
		total = total + 1
	else
		valueExists = false
	end if
loop

for i = 0 to 7
	fRandom = fRandom & myRnd(i)
next

end Function

%>
