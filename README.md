# T4-PR1-CristianSala

## Project overview
### Mobile view has a navbar expand feature
![img.png](img/img1.png)

### Desktop view
![img](img/img2.png)

![img](img/img3.png)

![img](img/img4.png)

![img](img/img5.png)

![img](img/img6.png)

![img](img/img7.png)

![img](img/img8.png)

![img](img/img9.png)

![img](img/img10.png)

![img](img/img11.png)

![img](img/img12.png)

Already had it done prior to the exercise modification, thus it has the original requirements 

## Class diagram
![Class-Diagram](img/img_cd.png)

## SonarCube-SonarScanner
### Commands to run after settig up a project
```batch 
dotnet sonarscanner begin /k:"T4PR1" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="sqp_f5866235d6cd48bfaade4cd83455604bab17bd84"
dotnet build
dotnet sonarscanner end /d:sonar.login="sqp_f5866235d6cd48bfaade4cd83455604bab17bd84"
```

### Results
![OverallCode](img/img_sq.png)

![issues](img/img_issues.png)