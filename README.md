# ClearBank

I followed the typical best coding practices when refactoring legacy code;

1. I refactored the code to make it testable
2. I covered the method with all of the necessary characterisation acceptance tests to have the ability to refactor safely 
3. I incrementally refactored the code towards a better solution
4. I refactored the characterisation test to reduce complexity by exercising only interactions with its immediate collaborators
5. All of the above was done with frequent commits that can be seen here:

> [master ≡]> git log --pretty=oneline
df3611729411b6d5aaa58e6edf81e63534a10509 Refactor and reduce complexity of payment service unit test to exercise only interaction with collaborators and remove the redundent test code that now exist in tests of internal collaborators
176a9a431fa5dfd4f509ece020e94ed18f254b0e Refactor payment service to inline the method call to validate a payment and use factory method of makepaymentresult to make the code more readable
bbc59c17b8efce8833da1a839038812c5b592a5a Add payment validation service collaborator into payment service
e2d04a486606874e83bb7394947bc50283deb5aa Introduce payment validation service and its unit tests
1ae92a1b523a112c80e50385059706df5484707f Introduce payment validator factory
a9d4ffb94b2fbfa18e77139cfe09c24e599c496f Introduce chaps payments validator
2c9b87efb33c458ca3f14f16c6d3d6ebee3d0d4a Introduce faster payments validator
689d30d3661010f69ff973813bbd52b85dd0079a Introduce bacs payment validator
3ea0cde882f0f84658a5d58e612e7e3284dbd572 Introduce account service and move the verification of the deducted balance from payment service to account service and its tests
7ec66bb501c0c684a0962bc06f262a89b695e1db Add characterization tests for PaymentService, remove the creation logic of data store out of the payment service and modify PaymentResult to return success when constructed (assuming bug in the test assignment, since all branches return false)
cdd04090309c85519510546df3b5b7d0116966d4 Add AccountDataStoreFactory and its tests
71b12f906e8ef63e3165d25518d35b09d966c0c2 Introduce simple refactorings for method to be testable to be able to put the method under characterization test
76a7b182ec508826ba1fa5e414ab06b2ce890390 Add initial source code of the assignment
30c0949e4e89146dbdb8c565d5378f0987f38637 Add gitignore
1a8b2572aff22785d52c4e07ff9d67151ea141f7 Initial commit



