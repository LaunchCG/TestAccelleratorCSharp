Feature: PurchaseItems
Purchase Items from an E-Commerce app

@SmokeBDD
Scenario: Purchase items
	Given I open the browser and enter url
	And I enter username and password
		| Username      | Password     |
		| standard_user | secret_sauce |
	And I click on Login button
	And I verify the title of homepage
	When I select Item1 and add to cart
	And I select Item2 and add to cart
	And I verify the total items in the cart
	And I click on Cart icon
	And I verify the items before checking out
	And I click on Checkout button
	And i verify the title in the checkout screen
	And I enter user information
		| Firstname | Lastname | PostalCode |
		| Siva      | A        | 635109     |
	And I click on Continue button
	And I verify the Shipping Payment info and Total cost
	And I Click on Finish button
	And I verify the order placed
	Then I logout from the app

@SmokeBDD
Scenario Outline: Invalid login
	Given I open the browser and enter url
	And I enter invalid "<username>" and "<password>"
	And I click on Login button
	

Examples:
	| username        | password     |
	| locked_out_user | secret_sauce |
	| problem_user    | secret_sauce |
